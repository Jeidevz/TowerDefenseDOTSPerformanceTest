using System.Numerics;
using TowerDefenseDOTS;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TowerDefenseDOTS
{
    [DisableAutoCreation]
    public class MoveTargetLocalPositionSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem endSys;

        protected override void OnCreate()
        {
            endSys = EntityManager.World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            EntityCommandBuffer.ParallelWriter commandBuffer = endSys.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref Translation translation, in MoveTargetLocalPosition moveData) =>
            {
                translation.Value = math.lerp(translation.Value, moveData.targetLocalPosition, moveData.speed);
                float distance = math.distance(translation.Value, moveData.targetLocalPosition);
                if(distance < 0.01f)
                {
                    translation.Value = moveData.targetLocalPosition;
                    commandBuffer.RemoveComponent<MoveTargetLocalPosition>(entityInQueryIndex, entity);
                }    

            }).Schedule();

            endSys.AddJobHandleForProducer(Dependency);
        }
    }
}