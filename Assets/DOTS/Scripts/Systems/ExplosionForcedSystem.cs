using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics.Extensions;
using Unity.Physics;

namespace TowerDefenseDOTS
{
    //[DisableAutoCreation]
    [UpdateAfter(typeof(EnemyCapsuleHealthSystem))]
    public class ExplosionForcedSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;

            JobHandle applyForceHandle = Entities.ForEach((ref PhysicsVelocity velocity, in PhysicsMass mass, in PhysicsCollider collider, in Translation translation,
                in Rotation rotation, in ExplosionForced forceData) =>
                {
                    velocity.ApplyExplosionForce(in mass, in collider, in translation, in rotation, forceData.force,
                        forceData.point - math.up(), forceData.radius, in dt, forceData.upForce, forceData.upwardsModifier, ForceMode.Force);

                }).Schedule(Dependency);


            applyForceHandle.Complete();

            EntityManager.RemoveComponent(GetEntityQuery(ComponentType.ReadOnly<ExplosionForced>()), typeof(ExplosionForced));
        }
    }
}