using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

namespace TowerDefenseDOTS
{
    public class DamageableProjectileSystem : SystemBase
    {
        public Unity.Physics.Systems.BuildPhysicsWorld physicsWorldSystem;
        public EndSimulationEntityCommandBufferSystem endSimCommandBuffSystem;
        public BeginSimulationEntityCommandBufferSystem beginSimCommandBuffSystem;

        protected override void OnCreate()
        {
            physicsWorldSystem = World.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>();
            endSimCommandBuffSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
            beginSimCommandBuffSystem = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            CollisionWorld collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;
            EntityCommandBuffer beginCommandBuffer = beginSimCommandBuffSystem.CreateCommandBuffer();
            EntityCommandBuffer endCommandBuffer = endSimCommandBuffSystem.CreateCommandBuffer();


            Entities.ForEach((Entity entity, int entityInQueryIndex, ref Translation translation,  ref DamageableProjectile projectile, in Rotation rotation) =>
            {
                RaycastInput input = new RaycastInput()
                {
                    Start = projectile.previousPosition,
                    End = translation.Value,
                    Filter = new CollisionFilter()
                    {
                        BelongsTo = projectile.colliderBelongsTo.Value,
                        CollidesWith = projectile.colliderCollidesWith.Value,
                        GroupIndex = 0
                    }
                };

                RaycastHit hit = new RaycastHit();
                bool haveHit = collisionWorld.CastRay(input, out hit);
                if (haveHit)
                {
                    // see hit.Position
                    // see hit.SurfaceNormal
                    //Entity e = physicsWorldSystem.PhysicsWorld.Bodies[hit.RigidBodyIndex].Entity;
                    Entity hitEntity = hit.Entity;
                    beginCommandBuffer.DestroyEntity(entity);
                    endCommandBuffer.AddComponent<Damaged>(hitEntity, new Damaged {value = projectile.damage});

                    Entity vfxEntity = endCommandBuffer.CreateEntity();
                    endCommandBuffer.AddComponent<VFX>(vfxEntity, new VFX
                    {
                        effect = VFXLibraryScriptableObject.Effect.BulletHit,
                        normal = hit.SurfaceNormal,
                        position = hit.Position
                    });

                    Entity sfxEntity = endCommandBuffer.CreateEntity();
                    endCommandBuffer.AddComponent<SFX>(sfxEntity, new SFX
                    {
                        clip = SoundLibraryScriptableObject.Clip.BulletHit,
                        position = hit.Position
                    });

                    endCommandBuffer.AddComponent<ImpulseComponent>(hit.Entity, new ImpulseComponent
                    {
                        point = hit.Position,
                        force = 30f,
                        normal = hit.SurfaceNormal,
                        upwardMultiplier = 1
                    });

                }
                else
                {
                    projectile.previousPosition = translation.Value;
                }
            }).Schedule();

            beginSimCommandBuffSystem.AddJobHandleForProducer(Dependency);
            endSimCommandBuffSystem.AddJobHandleForProducer(Dependency);
        }

        [BurstCompile]
        struct RaycastFillInputsJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<DamageableProjectile> damageableProjectiles;
            [ReadOnly]
            public NativeArray<Translation> translations;
            public NativeArray<RaycastInput> raycastInputs;
            public void Execute(int index)
            {

                raycastInputs[index] = new RaycastInput()
                {
                    Start = damageableProjectiles[index].previousPosition,
                    End = translations[index].Value,
                    Filter = new CollisionFilter()
                    {
                        BelongsTo = damageableProjectiles[index].colliderBelongsTo.Value,
                        CollidesWith = damageableProjectiles[index].colliderCollidesWith.Value,
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

    }
}
