using TowerDefenseDOTS;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

namespace TowerDefenseDOTS
{
    public class EnemyCapsuleHealthSystem : SystemBase
    {
        EndFixedStepSimulationEntityCommandBufferSystem endSimSys;

        protected override void OnCreate()
        {
            endSimSys = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
        }
        protected override void OnUpdate()
        {
            EntityCommandBuffer.ParallelWriter commandBuffer = endSimSys.CreateCommandBuffer().AsParallelWriter();

            Entities
                .WithAny<Tag_EnemyCapsule>()
                .ForEach((Entity entity, int entityInQueryIndex, ref PhysicsMass mass, in Health health, in LocalToWorld transform) =>
            {
                if (health.value <= 0)
                {
                    MassProperties massProp = new MassProperties
                    {
                        AngularExpansionFactor = 1f, //Resistance to rotation force. Higher value means hard to rotate.
                        MassDistribution = new MassDistribution
                        {
                            InertiaTensor = new float3(1f, 1f, 1f), //Affect to send flying
                            Transform = new RigidTransform { pos = transform.Position, rot = transform.Rotation }
                        },
                        Volume = 1f
                    };

                    mass = PhysicsMass.CreateDynamic(massProp, 60f);
                    mass.CenterOfMass = float3.zero; // Important!

                    commandBuffer.AddComponent<Lifetime>(entityInQueryIndex, entity, new Lifetime { value = 5f });
                    commandBuffer.AddComponent<PhysicsGravityFactor>(entityInQueryIndex, entity, new PhysicsGravityFactor { Value = 9.81f });
                    commandBuffer.RemoveComponent<ForwardMovable>(entityInQueryIndex ,entity);
                    commandBuffer.RemoveComponent<Health>(entityInQueryIndex, entity);
                }
            }).ScheduleParallel();
        }
    }
}