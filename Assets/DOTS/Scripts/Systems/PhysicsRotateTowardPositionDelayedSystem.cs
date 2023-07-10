using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TowerDefenseDOTS
{
    public class PhysicsRotateTowardPositionDelayedSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem endSimSys;

        protected override void OnCreate()
        {
            endSimSys = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            EntityCommandBuffer commandBuffer = endSimSys.CreateCommandBuffer();

            Entities.ForEach((Entity entity, ref PhysicsRotateTowardPositionDelayed rotateDelayData) =>
            {
                if (rotateDelayData.delay <= 0)
                {
                    commandBuffer.RemoveComponent<PhysicsRotateTowardPositionDelayed>(entity);
                    commandBuffer.AddComponent<PhysicsRotateTowardPosition>(entity, rotateDelayData.physicsRotate);
                }
                else
                    rotateDelayData.delay -= dt;
                
            }).Schedule();
        }
    }
}