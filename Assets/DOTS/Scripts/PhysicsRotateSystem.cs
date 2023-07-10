using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Extensions;

namespace TowerDefenseDOTS
{
    public class PhysicsRotateSystem : SystemBase
    {
        protected override void OnUpdate()
        {

            Entities.ForEach((ref Translation translation, ref PhysicsVelocity velocity, in PhysicsMass mass, in Rotation rotation, in PhysicsRotate rotateData) =>
            {
                velocity.SetAngularVelocityWorldSpace(mass, rotation, rotateData.value);
            }).Schedule();
        }
    }
}