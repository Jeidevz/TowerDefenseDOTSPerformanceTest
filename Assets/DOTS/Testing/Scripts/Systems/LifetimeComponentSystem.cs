using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Testing
{
    public class LifetimeComponentSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem beginSimSys;

        protected override void OnCreate()
        {
            beginSimSys = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            EntityCommandBuffer.ParallelWriter beginCommandBufferParal = beginSimSys.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref LifetimeComponent lifetimeComponent) => {
                lifetimeComponent.time -= dt;

                if(lifetimeComponent.time < 0)
                {
                    beginCommandBufferParal.DestroyEntity(entityInQueryIndex, entity);
                }

            }).ScheduleParallel();

            beginSimSys.AddJobHandleForProducer(Dependency);
        }
    }
}