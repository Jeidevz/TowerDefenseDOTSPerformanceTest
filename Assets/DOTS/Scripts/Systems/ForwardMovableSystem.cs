using TowerDefenseDOTS;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Unity.Physics.Systems;

namespace TowerDefenseDOTS
{
    [UpdateInGroup(typeof(PhysicChangesSystemGroup))]
    public class ForwardMovableSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;

            Entities.ForEach((ref Translation translation, in Rotation rotation, in ForwardMovable forwardMvble) =>
            {
                float3 forwardDir = math.mul(rotation.Value, new float3(0f, 0f, 1f));
                translation.Value += forwardDir * forwardMvble.speed * dt;
            }).ScheduleParallel();
        }
    }
}