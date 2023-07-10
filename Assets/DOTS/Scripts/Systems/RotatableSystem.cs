using System.Numerics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;

namespace TowerDefenseDOTS
{
    public class RotatableSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;



            Entities.ForEach((ref Rotation rotation, in Rotatable rotatable) =>
            {
                float3 rotate = math.normalizesafe(rotatable.rotate) * rotatable.speed * dt;
                rotation.Value = math.mul(rotation.Value, quaternion.RotateX(rotate.x));
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(rotate.y));
                rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(rotate.z));
            }).Schedule();
        }
    }
}