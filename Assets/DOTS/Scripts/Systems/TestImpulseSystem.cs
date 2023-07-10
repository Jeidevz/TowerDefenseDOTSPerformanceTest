using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Unity.Physics.Extensions;

namespace TowerDefenseDOTS
{ 
[DisableAutoCreation]
public class TestImpulseSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PhysicsMass mass, ref PhysicsVelocity velocity, in Tag_EnemyCapsule tag, in Translation translation, in Rotation rotation) => {
            ApplyImpulse(ref velocity, mass, translation, rotation, new float3(0, 103, 0), translation.Value);
            //float3 impulse = new float3(0, 103f, 0);
            //velocity.ApplyLinearImpulse(in mass, in impulse);
            //velocity.ApplyAngularImpulse(in mass, in impulse);
        }).Schedule();



    }

    public static void ApplyImpulse(ref PhysicsVelocity pv, PhysicsMass pm,
        Translation t, Rotation r, float3 impulse, float3 point)
    {
        // Linear
        pv.Linear += impulse;

            // Angular
            {
                // Calculate point impulse
                var worldFromEntity = new RigidTransform(r.Value, t.Value);
                var worldFromMotion = math.mul(worldFromEntity, pm.Transform);
                float3 angularImpulseWorldSpace = math.cross(point - worldFromMotion.pos, impulse);
                float3 angularImpulseInertiaSpace = math.rotate(math.inverse(worldFromMotion.rot), angularImpulseWorldSpace);

                pv.Angular += angularImpulseInertiaSpace * pm.InverseInertia;
            }      }
    }
}
