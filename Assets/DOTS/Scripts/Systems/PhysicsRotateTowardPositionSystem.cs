using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using UnityEngine;
using Unity.Physics.Extensions;
using Unity.Physics.Systems;

namespace TowerDefenseDOTS
{
    [UpdateInGroup(typeof(PhysicChangesSystemGroup))]
    public class PhysicsRotateTowardPositionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;

            Entities.ForEach((ref Translation translation, ref PhysicsVelocity velocity,
                ref Rotation rotation, in PhysicsMass mass, in LocalToWorld transform, in PhysicsRotateTowardPosition rotateData) => {
                    float3 direction = Vector3.Normalize(rotateData.position - translation.Value);
                    rotation.Value = quaternion.LookRotationSafe(direction, math.up());
                }).ScheduleParallel();

        }
    }
}