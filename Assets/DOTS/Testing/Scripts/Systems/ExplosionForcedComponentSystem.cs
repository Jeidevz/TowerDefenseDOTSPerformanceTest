using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics.Extensions;
using Unity.Physics;

namespace Testing
{
    public class ExplosionForcedComponentSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem beginCommandBufferSys;
        protected override void OnCreate()
        {
            beginCommandBufferSys = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        }
        protected override void OnUpdate()
        {
            EntityCommandBuffer.ParallelWriter beginCommandParal = beginCommandBufferSys.CreateCommandBuffer()
                .AsParallelWriter();

            float dt = Time.DeltaTime;

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref PhysicsVelocity velocity, 
                in PhysicsMass mass, in PhysicsCollider collider, in Translation translation,
                in Rotation rotation, in ExplosionForcedComponent forceData) =>
                {
                    velocity.ApplyExplosionForce(in mass, in collider, in translation, 
                        in rotation, forceData.force,
                        forceData.point - math.up(), forceData.radius, in dt, forceData.upForce, 
                        forceData.upwardsModifier, ForceMode.Force);

                    beginCommandParal.RemoveComponent<ExplosionForcedComponent>(entityInQueryIndex, entity);
                }).ScheduleParallel();
            
            beginCommandBufferSys.AddJobHandleForProducer(Dependency);
        }
    }
}

