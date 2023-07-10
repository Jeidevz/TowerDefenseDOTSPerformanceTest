using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace TowerDefenseDOTS
{
    [UpdateInGroup(typeof(PhysicChangesSystemGroup))]
    public class ExplosiveSystem : SystemBase
    {
        public struct ExplosionSetting
        {
            public float damage;
            public float force;
            public float radius;
            public float upForce;
            public float upwardsModifier;
        }

        BuildPhysicsWorld buildPhysicsWorld;
        StepPhysicsWorld stepPhysicsWorld;
        BeginSimulationEntityCommandBufferSystem beginSimSys;
        EndSimulationEntityCommandBufferSystem endSimSys;

        protected override void OnCreate()
        {
            buildPhysicsWorld = World.GetExistingSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
            beginSimSys = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            endSimSys = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }


        protected override void OnUpdate()
        {
            EntityCommandBuffer beginCommandBuffer = beginSimSys.CreateCommandBuffer();
            EntityCommandBuffer endCommandBuffer = endSimSys.CreateCommandBuffer();
            ComponentDataFromEntity<ExplosiveComponent> explosiveComponents = GetComponentDataFromEntity<ExplosiveComponent>(true);

            CollisionExplosionJob collisionExplosionJob = new CollisionExplosionJob()
            {
                explosiveGroup = explosiveComponents,
                beginCommandBuffer = beginCommandBuffer,
                endCommandBuffer = endCommandBuffer,
                physicsWorld = buildPhysicsWorld.PhysicsWorld
            };

            JobHandle collisionExplosionHandle = collisionExplosionJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);
            collisionExplosionHandle.Complete();
            beginSimSys.AddJobHandleForProducer(Dependency);
        }

        [BurstCompile]
        private struct TriggerExplosionJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<ExplosiveComponent> explosiveGroup;
            [ReadOnly] public ComponentDataFromEntity<Tag_EnemyCapsule> enemyGroup;
            public EntityCommandBuffer beginSimCommandBuffer;
            public void Execute(TriggerEvent triggerEvent)
            {
                Entity entityA = triggerEvent.EntityA;
                Entity entityB = triggerEvent.EntityB;

                bool entityAIsExplosive = explosiveGroup.HasComponent(entityA);
                bool entityBIsExplosive = explosiveGroup.HasComponent(entityB);

                if (entityAIsExplosive)
                    beginSimCommandBuffer.DestroyEntity(entityA);

                if (entityBIsExplosive)
                    beginSimCommandBuffer.DestroyEntity(entityB);

            }
        }

        [BurstCompile]
        private struct CollisionExplosionJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<ExplosiveComponent> explosiveGroup;
            public EntityCommandBuffer beginCommandBuffer;
            public EntityCommandBuffer endCommandBuffer;
            [ReadOnly] public PhysicsWorld physicsWorld;

            public void Execute(CollisionEvent collisionEvent)
            {
                Entity entityA = collisionEvent.EntityA;
                Entity entityB = collisionEvent.EntityB;

                bool entityAIsExplosive = explosiveGroup.HasComponent(entityA);
                bool entityBIsExplosive = explosiveGroup.HasComponent(entityB);

                var details = collisionEvent.CalculateDetails(ref physicsWorld);
                float3 contactPosition = details.AverageContactPointPosition;
                float3 normal = collisionEvent.Normal;

                if (entityAIsExplosive)
                {
                    ExplosiveComponent explosiveData = explosiveGroup[entityA];
                    beginCommandBuffer.DestroyEntity(entityA);
                    Explode(ref endCommandBuffer, in  contactPosition, in normal, in explosiveData);
                }

                if (entityBIsExplosive)
                {
                    ExplosiveComponent explosiveData = explosiveGroup[entityB];
                    beginCommandBuffer.DestroyEntity(entityB);
                    Explode(ref endCommandBuffer, in contactPosition, in normal, in explosiveData);
                }
            }
        }


        public static void Explode(ref EntityCommandBuffer commandBuffer, in float3 position, in float3 normal, in ExplosiveComponent explosiveData)
        {
            Entity explosiveAreaEntity = commandBuffer.CreateEntity();
            commandBuffer.AddComponent<ExplodedArea>(explosiveAreaEntity, new ExplodedArea {
                explosive = explosiveData,
                point = position
            });

            Entity vfxEntity = commandBuffer.CreateEntity();
            commandBuffer.AddComponent<VFX>(vfxEntity, new VFX {
                normal = normal,
                effect = VFXLibraryScriptableObject.Effect.MissileExplosion,
                parent = Entity.Null,
                position = position
            });

            Entity sfxEntity = commandBuffer.CreateEntity();
            commandBuffer.AddComponent<SFX>(sfxEntity, new SFX {
                position = position,
                clip = SoundLibraryScriptableObject.Clip.MissileExplosion,
                parent = Entity.Null
            });
        }
    }
}