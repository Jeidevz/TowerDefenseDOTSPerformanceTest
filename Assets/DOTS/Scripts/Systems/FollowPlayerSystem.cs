using TowerDefenseDOTS;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TowerDefenseDOTS
{
    [UpdateAfter(typeof(PlayerTurrentInputSystem))]
    public class FollowPlayerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float3 position = float3.zero;
            Entities.ForEach((ref Translation translation, in Tag_Player tag) =>
            {
                position = translation.Value;
            }).Run();

            Entities.ForEach((ref Translation translation, in FollowPlayerComponent followPlayerComponent) =>
            {
                translation.Value = position;
            }).Run();
        }
    }
}