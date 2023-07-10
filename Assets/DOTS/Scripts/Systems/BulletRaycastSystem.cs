using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics;
using Unity.Rendering;
using UnityEditor;

namespace TowerDefenseDOTS
{
    [DisableAutoCreation]
    //[UpdateInGroup(typeof(PhysicChangesSystemGroup)), UpdateBefore(typeof(BulletSystem))]
    [UpdateAfter(typeof(MovableSystem))]
    public class BulletRaycastSystem : SystemBase
    {
        public Unity.Physics.Systems.BuildPhysicsWorld physicsWorldSystem;
        public EndSimulationEntityCommandBufferSystem endSimCommandBufferSystem;
        public BeginSimulationEntityCommandBufferSystem beginSimCommandBufferSystem;
        public BulletSystem bulletSystem;
        

        protected override void OnCreate()
        {
            base.OnCreate();
            physicsWorldSystem = World.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>();
            endSimCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            beginSimCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            bulletSystem = World.GetOrCreateSystem<BulletSystem>();
        }
        protected override void OnUpdate()
        {
            var collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;
            EntityCommandBuffer.ParallelWriter endSimCommandBuffer = endSimCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            EntityCommandBuffer.ParallelWriter beginSimCommandBuffer = beginSimCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

            EntityQueryDesc queryDesc = new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(BulletComponent), ComponentType.ReadOnly<Tag_Bullet>(), ComponentType.ReadOnly<Translation>()}
            };
            EntityQuery query = GetEntityQuery(queryDesc);
            int count = query.CalculateEntityCount();

            NativeArray<RaycastInput> raycastInputs = new NativeArray<RaycastInput>( count, Allocator.TempJob);
            NativeArray<Entity> bulletEntities = query.ToEntityArray(Allocator.TempJob);
            NativeArray<BulletComponent> bulletComponents = query.ToComponentDataArray<BulletComponent>(Allocator.TempJob);
            NativeArray<Translation> translations = query.ToComponentDataArray<Translation>(Allocator.TempJob);

            RaycastFillInputsJob raycastInputsFillJob = new RaycastFillInputsJob()
            {
                bulletComponents = bulletComponents,
                translations = translations,
                raycastInputs = raycastInputs
            };

            ApplyDamageToHittedJob applyDamageToHittedJob = new ApplyDamageToHittedJob()
            {
                inputs = raycastInputs,
                collisionWorld = collisionWorld,
                bulletComponents = bulletComponents,
                endSimCommandBuffer = endSimCommandBuffer,
                bulletEntities = bulletEntities,
                beginSimCommandBuffer = beginSimCommandBuffer,
                translations = translations
                
            };

            JobHandle raycastInputsFillHandle = raycastInputsFillJob.Schedule(count, 8);
            JobHandle applyDmgHittedHandle = applyDamageToHittedJob.Schedule(count, 8, raycastInputsFillHandle);
            applyDmgHittedHandle.Complete();

            //Update manually bullet new previous position
            bulletSystem.Update();
            

            raycastInputs.Dispose();
            bulletComponents.Dispose();
            translations.Dispose();
            bulletEntities.Dispose();
            endSimCommandBufferSystem.AddJobHandleForProducer(Dependency);
            beginSimCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }

        [BurstCompile]
        struct RaycastFillCommandsJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<BulletComponent> bulletComponents;
            [ReadOnly]
            public NativeArray<Translation> translations;
            public NativeArray<RaycastCommand> commands;

            public void Execute(int i)
            {
                float3 direction = math.normalizesafe(bulletComponents[i].previousPosition - translations[i].Value);
                float distance = Vector3.Magnitude(bulletComponents[i].previousPosition - translations[i].Value);
                commands[i] = new RaycastCommand(translations[i].Value, direction, distance);
            }
        }

        [BurstCompile]
        struct RaycastFillInputsJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<BulletComponent> bulletComponents;
            [ReadOnly]
            public NativeArray<Translation> translations;
            public NativeArray<RaycastInput> raycastInputs;
            public void Execute(int index)
            {

                raycastInputs[index] = new RaycastInput()
                {
                    Start = bulletComponents[index].previousPosition,
                    End = translations[index].Value,
                    Filter = new CollisionFilter()
                    {
                        BelongsTo = bulletComponents[index].colliderBelongsTo.Value,
                        CollidesWith = bulletComponents[index].colliderCollidesWith.Value,
                        GroupIndex = 0
                    }
                };
            }
        }

        [BurstCompile]
        struct ApplyDamageToHittedJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<Entity> bulletEntities;
            [ReadOnly] public CollisionWorld collisionWorld;
            [ReadOnly] public NativeArray<RaycastInput> inputs;
            public NativeArray<BulletComponent> bulletComponents;
            public NativeArray<Translation> translations;
            public EntityCommandBuffer.ParallelWriter endSimCommandBuffer;
            public EntityCommandBuffer.ParallelWriter beginSimCommandBuffer;

            public void Execute(int index)
            {
                Unity.Physics.RaycastHit hit;
                if (collisionWorld.CastRay(inputs[index], out hit))
                {
                    //endSimCommandBuffer.AddComponent<Damaged>(index, hit.Entity, new Damaged { value = bulletComponents[index].damage });

                    Entity vfxEntity = endSimCommandBuffer.CreateEntity(index);
                    endSimCommandBuffer.AddComponent<VFX>(index, vfxEntity, new VFX
                    {
                        effect = VFXLibraryScriptableObject.Effect.BulletHit,
                        normal = hit.SurfaceNormal,
                        position = hit.Position
                    });

                    Entity sfxEntity = endSimCommandBuffer.CreateEntity(index);
                    endSimCommandBuffer.AddComponent<SFX>(index, sfxEntity, new SFX
                    {
                        clip = SoundLibraryScriptableObject.Clip.BulletHit,
                        position = hit.Position
                    });

                    //endSimCommandBuffer.AddComponent<ImpulseComponent>(index, hit.Entity, new ImpulseComponent
                    //{
                    //    point = hit.Position,
                    //    force = 30f,
                    //    normal = hit.SurfaceNormal,
                    //    upwardMultiplier = 1
                    //});

                    beginSimCommandBuffer.DestroyEntity(index, bulletEntities[index]);
                }
            }
        }

        [BurstCompile]
        struct RaycastInputsHitDeteactionsJob : IJobParallelFor
        {
            public NativeArray<RaycastInput> inputs;
            public NativeArray<Unity.Physics.RaycastHit> hits;
            [ReadOnly]
            public CollisionWorld collisionWorld;

            public void Execute(int index)
            {
                Unity.Physics.RaycastHit hit;
                collisionWorld.CastRay(inputs[index], out hit);
                hits[index] = hit;
            }
        }

        [BurstCompile]
        struct DestroyHittedBulletEntities : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<Unity.Physics.RaycastHit> hits;
            [ReadOnly]
            public NativeArray<Entity> bulletEntities;
            public EntityCommandBuffer.ParallelWriter ecbParallelWriter;

            public void Execute(int index)
            {
                
                if (hits[index].Entity != Entity.Null)
                {
                    ecbParallelWriter.DestroyEntity(index, bulletEntities[index]);
                    Entity vfxEntity = ecbParallelWriter.CreateEntity(index);
                    ecbParallelWriter.AddComponent<VFX>(index, vfxEntity, new VFX
                    {
                        effect = VFXLibraryScriptableObject.Effect.BulletHit,
                        normal = hits[index].SurfaceNormal,
                        position = hits[index].Position
                    });

                    Entity sfxEntity = ecbParallelWriter.CreateEntity(index);
                    ecbParallelWriter.AddComponent<SFX>(index, sfxEntity, new SFX
                    {
                        clip = SoundLibraryScriptableObject.Clip.BulletHit,
                        position = hits[index].Position
                    });
                }
            }
        }
    }
}