//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;
//using Unity.Transforms;
//using UnityEngine;
//using Unity.Physics;
//using Unity.Rendering;

//namespace TowerDefenseDOTS
//{
//    [UpdateBefore(typeof(EnemyCapsuleHealthSystem))]
//    public class BulletRaycastSystem : SystemBase
//    {
//        public Unity.Physics.Systems.BuildPhysicsWorld physicsWorldSystem;
//        public EntityCommandBufferSystem ecbSystem;

//        protected override void OnCreate()
//        {
//            base.OnCreate();
//            physicsWorldSystem = World.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>();
//            ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
//        }
//        protected override void OnUpdate()
//        {
//            var collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;
//            EntityCommandBuffer.ParallelWriter ecbParallelWriter = ecbSystem.CreateCommandBuffer().AsParallelWriter();

//            EntityQueryDesc queryDesc = new EntityQueryDesc
//            {
//                All = new ComponentType[] { typeof(BulletComponent), ComponentType.ReadOnly<Tag_Bullet>(), ComponentType.ReadOnly<Translation>()}
//            };
//            EntityQuery query = GetEntityQuery(queryDesc);
//            int count = query.CalculateEntityCount();

//            NativeArray<RaycastInput> raycastInputs = new NativeArray<RaycastInput>( count, Allocator.TempJob);
//            NativeArray<Unity.Physics.RaycastHit> raycastHits = new NativeArray<Unity.Physics.RaycastHit>( count, Allocator.TempJob);
//            NativeArray<Entity> bulletEntities = query.ToEntityArray(Allocator.TempJob);
//            NativeArray<BulletComponent> bulletComponents = query.ToComponentDataArray<BulletComponent>(Allocator.TempJob);
//            NativeArray<Translation> translations = query.ToComponentDataArray<Translation>(Allocator.TempJob);

//            RaycastFillInputsJob raycastInputsFillJob = new RaycastFillInputsJob()
//            {
//                bulletComponents = bulletComponents,
//                translations = translations,
//                raycastInputs = raycastInputs
//            };

//            RaycastInputsHitDeteactionsJob raycastHitDetectJob = new RaycastInputsHitDeteactionsJob()
//            {
//                inputs = raycastInputs,
//                hits = raycastHits,
//                collisionWorld = collisionWorld
//            };

//            DestroyHittedBulletEntities destroyHittedBulletEntitiesJob = new DestroyHittedBulletEntities()
//            {
//                hits = raycastHits,
//                bulletEntities = bulletEntities,
//                ecbParallelWriter = ecbParallelWriter
//            };

//            JobHandle raycastInputsFillHandle = raycastInputsFillJob.Schedule(count, 32);
//            JobHandle rayHitDetectHandle = raycastHitDetectJob.Schedule(count, 32, raycastInputsFillHandle);
//            rayHitDetectHandle.Complete();

//            //Add death timer those got hit
//            //Heavy load cause running in mainthread
//            foreach (Unity.Physics.RaycastHit hit in raycastHits)
//            {
//                if (hit.Entity != Entity.Null && EntityManager.Exists(hit.Entity) && !EntityManager.HasComponent<Lifetime>(hit.Entity) && EntityManager.HasComponent<PhysicsVelocity>(hit.Entity))
//                {
//                    if (HasComponent<Health>(hit.Entity))
//                    {
//                        Health health = EntityManager.GetComponentData<Health>(hit.Entity);
//                        health.value -= 100;
//                        EntityManager.SetComponentData<Health>(hit.Entity, health);
//                    }

//                    ////TODO: Manually added bullet damaged. Need to change to use from the bullet component data.
//                    //EntityManager.AddComponentData<Damaged>(hit.Entity, new Damaged { value = 100f });

//                    EntityManager.AddComponentData<ImpulseComponent>(hit.Entity, new ImpulseComponent
//                    {
//                        point = hit.Position,
//                        force = 30f,
//                        normal = hit.SurfaceNormal,
//                        upwardMultiplier = 1
//                    });

//                    Entity vfxEntity = EntityManager.CreateEntity();
//                    EntityManager.AddComponentData<VFX>(vfxEntity, new VFX
//                    {
//                        effect = VFXLibraryScriptableObject.Effect.BulletHit,
//                        normal = hit.SurfaceNormal,
//                        position = hit.Position
//                    });

//                    Entity sfxEntity = EntityManager.CreateEntity();
//                    EntityManager.AddComponentData<SFX>(sfxEntity, new SFX
//                    {
//                        clip = SoundLibraryScriptableObject.Clip.BulletHit,
//                        position = hit.Position
//                    });
//                }
//            }

//            JobHandle destroyHittedBulletHandle = destroyHittedBulletEntitiesJob.Schedule(count, 32);
//            destroyHittedBulletHandle.Complete();

//            raycastHits.Dispose();
//            raycastInputs.Dispose();
//            bulletComponents.Dispose();
//            translations.Dispose();
//            bulletEntities.Dispose();
//            ecbSystem.AddJobHandleForProducer(Dependency);
//        }

//        [BurstCompile]
//        struct RaycastFillCommandsJob : IJobParallelFor
//        {
//            [ReadOnly]
//            public NativeArray<BulletComponent> bulletComponents;
//            [ReadOnly]
//            public NativeArray<Translation> translations;
//            public NativeArray<RaycastCommand> commands;

//            public void Execute(int i)
//            {
//                float3 direction = math.normalizesafe(bulletComponents[i].previousPosition - translations[i].Value);
//                float distance = Vector3.Magnitude(bulletComponents[i].previousPosition - translations[i].Value);
//                commands[i] = new RaycastCommand(translations[i].Value, direction, distance);
//            }
//        }

//        [BurstCompile]
//        struct RaycastFillInputsJob : IJobParallelFor
//        {
//            [ReadOnly]
//            public NativeArray<BulletComponent> bulletComponents;
//            [ReadOnly]
//            public NativeArray<Translation> translations;
//            public NativeArray<RaycastInput> raycastInputs;
//            public void Execute(int index)
//            {

//                raycastInputs[index] = new RaycastInput()
//                {
//                    Start = bulletComponents[index].previousPosition,
//                    End = translations[index].Value,
//                    Filter = new CollisionFilter()
//                    {
//                        BelongsTo = bulletComponents[index].colliderBelongsTo.Value,
//                        CollidesWith = bulletComponents[index].colliderCollidesWith.Value,
//                        GroupIndex = 0
//                    }
//                };
//            }
//        }

//        [BurstCompile]
//        struct RaycastInputsHitDeteactionsJob : IJobParallelFor
//        {
//            public NativeArray<RaycastInput> inputs;
//            public NativeArray<Unity.Physics.RaycastHit> hits;
//            [ReadOnly]
//            public CollisionWorld collisionWorld;

//            public void Execute(int index)
//            {
//                Unity.Physics.RaycastHit hit;
//                collisionWorld.CastRay(inputs[index], out hit);
//                hits[index] = hit;
//            }
//        }

//        [BurstCompile]
//        struct DestroyHittedBulletEntities : IJobParallelFor
//        {
//            [ReadOnly]
//            public NativeArray<Unity.Physics.RaycastHit> hits;
//            [ReadOnly]
//            public NativeArray<Entity> bulletEntities;
//            public EntityCommandBuffer.ParallelWriter ecbParallelWriter;

//            public void Execute(int index)
//            {
                
//                if (hits[index].Entity != Entity.Null)
//                {
//                    ecbParallelWriter.DestroyEntity(index, bulletEntities[index]);
//                    Entity vfxEntity = ecbParallelWriter.CreateEntity(index);
//                    ecbParallelWriter.AddComponent<VFX>(index, vfxEntity, new VFX
//                    {
//                        effect = VFXLibraryScriptableObject.Effect.BulletHit,
//                        normal = hits[index].SurfaceNormal,
//                        position = hits[index].Position
//                    });

//                    Entity sfxEntity = ecbParallelWriter.CreateEntity(index);
//                    ecbParallelWriter.AddComponent<SFX>(index, sfxEntity, new SFX
//                    {
//                        clip = SoundLibraryScriptableObject.Clip.BulletHit,
//                        position = hits[index].Position
//                    });
//                }
//            }
//        }
//    }
//}