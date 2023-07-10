using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TowerDefenseDOTS
{
    public class RotatingTowardTargetSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;

            Entities.ForEach((ref Translation translation, ref Rotation rotation, ref RotatingTowardTarget rotatingData) =>
            {
                float speed = rotatingData.speed * rotatingData.rotatedTime / 10f;
                float3 direction =  rotatingData.targetPosition - translation.Value;
                rotation.Value = Quaternion.Lerp(rotation.Value, Quaternion.LookRotation(direction, math.up()), speed);
                rotatingData.rotatedTime += dt;
            }).Schedule();
        }
    }
}