using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TowerDefenseDOTS
{
    public class LifetimeSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem beginSimSys;

        protected override void OnCreate()
        {
            beginSimSys = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;

            EntityCommandBuffer.ParallelWriter beginSimCommandBufferParal = beginSimSys.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref Lifetime lifetime) => {
                if (lifetime.value <= 0)
                {
                    beginSimCommandBufferParal.DestroyEntity(entityInQueryIndex, entity);
                }
                else
                {
                    lifetime.value -= dt;
                }
            }).ScheduleParallel();

            beginSimSys.AddJobHandleForProducer(Dependency);
        }
    }
}
