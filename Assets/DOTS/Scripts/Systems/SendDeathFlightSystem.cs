using TowerDefenseDOTS;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics.Extensions;
using Unity.Physics;
using System.Security;

namespace TowerDefenseDOTS
{
    [DisableAutoCreation]
    public class SendDeathFlightSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem esecbSystem;
        protected override void OnCreate()
        {
            base.OnCreate();
            esecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer.ParallelWriter ecbParalWriter = esecbSystem.CreateCommandBuffer().AsParallelWriter();


            Entities.ForEach((Entity entity, int entityInQueryIndex, ref PhysicsVelocity velocity, in Translation translation, in PhysicsMass mass,
                in Rotation rotation, in SendDeathFlightComponent sdfComp) =>
            {
                PhysicsComponentExtensions.ApplyImpulse(ref velocity, in mass, in translation, in rotation, -sdfComp.normal + sdfComp.impulse * sdfComp.force,
                    sdfComp.point);
                ecbParalWriter.AddComponent<Lifetime>(entityInQueryIndex, entity);
                ecbParalWriter.SetComponent<Lifetime>(entityInQueryIndex, entity, new Lifetime { value = sdfComp.deathTimer });
                ecbParalWriter.RemoveComponent<SendDeathFlightComponent>(entityInQueryIndex, entity);
            }).ScheduleParallel();

            esecbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}