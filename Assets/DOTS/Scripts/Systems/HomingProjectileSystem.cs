using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TowerDefenseDOTS
{
    public class HomingProjectileSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;

            Entities.ForEach((ref Translation translation, ref HomingProjectile projectile, ref Rotation rotation) =>
            {
                float distance = math.distance(translation.Value, projectile.targetPosition);
                if (projectile.delay <= 0 && projectile.stopHomingDistance < distance)
                {
                    float3 direction = Vector3.Normalize(projectile.targetPosition - translation.Value);
                    rotation.Value = quaternion.LookRotationSafe(direction, math.up());
                }
                else
                {
                    projectile.delay -= dt;
                }

            }).Schedule();
        }
    }
}