using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics.Systems;

namespace TowerDefenseDOTS
{
    public class MovableSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;

            Entities.ForEach((ref Translation translation, in Movable mov) =>
            {
                translation.Value += mov.direction * mov.speed * dt;
            }).ScheduleParallel();
        }
    }
}