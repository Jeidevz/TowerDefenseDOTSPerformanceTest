using System.CodeDom;
using TowerDefenseDOTS;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TowerDefenseDOTS
{
    [UpdateBefore(typeof(EnemyCapsuleHealthSystem))]
    public class DamagedSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem beginSimCommandBufferSys;
        protected override void OnCreate()
        {
            beginSimCommandBufferSys = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer beginCommandBuffer = beginSimCommandBufferSys.CreateCommandBuffer();

            Entities.ForEach((ref Health health, in Damaged damage) =>
            {
                health.value -= damage.value;
            }).Schedule();

            beginCommandBuffer.RemoveComponent(GetEntityQuery(ComponentType.ReadOnly<Damaged>()), typeof(Damaged));
            beginSimCommandBufferSys.AddJobHandleForProducer(Dependency);
        }
    }
}
