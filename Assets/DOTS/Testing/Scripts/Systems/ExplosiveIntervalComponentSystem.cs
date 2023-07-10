using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Testing
{
    public class ExplosiveIntervalComponentSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem beginSimSys;
        EndFixedStepSimulationEntityCommandBufferSystem endSimSys;
        protected override void OnCreate()
        {
            beginSimSys = World.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
            endSimSys = World.GetExistingSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            EntityCommandBuffer.ParallelWriter beginBufferParal = beginSimSys
                .CreateCommandBuffer().AsParallelWriter();
            EntityCommandBuffer.ParallelWriter endBufferParal = endSimSys
                .CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, int entityInQueryIndex, 
                ref ExplosiveIntervalComponent explosiveCountdownComponent, 
                in Translation translation) =>
            {
                explosiveCountdownComponent.timer -= dt;

                if(explosiveCountdownComponent.timer < 0)
                {
                    Entity explodedAreaEntity = endBufferParal.CreateEntity(entityInQueryIndex);
                    endBufferParal.AddComponent(entityInQueryIndex, explodedAreaEntity, 
                        new ExplodedAreaComponent 
                        { 
                            position = translation.Value,
                            explosionData = explosiveCountdownComponent.explosionData
                        });

                    explosiveCountdownComponent.timer = explosiveCountdownComponent.defaultInterval;
                }
            }).ScheduleParallel();

            beginSimSys.AddJobHandleForProducer(Dependency);
        }
    }
}


