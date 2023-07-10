using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TowerDefenseDOTS
{
    public class LookDirectionVerticallySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Rotation rotation, in Translation translation, in LocalToWorld transform, in LookDirectionVertically lookData) =>
            {
                float3 forwardDir = math.mul(rotation.Value, new float3(0f, 0f, 1f));
                float3 direction = math.normalizesafe((new float3(forwardDir.x, lookData.direction.y, forwardDir.z)));

                rotation.Value = quaternion.LookRotation(direction, math.up());
            }).Schedule();
        }
    }
}