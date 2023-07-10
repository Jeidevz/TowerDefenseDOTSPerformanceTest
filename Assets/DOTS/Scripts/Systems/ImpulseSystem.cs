using System.CodeDom;
using TowerDefenseDOTS;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Physics.Extensions;
using Unity.Transforms;

namespace TowerDefenseDOTS
{
    [UpdateInGroup(typeof(PhysicChangesSystemGroup))]
    public class ImpulseSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem endSimEntComSystem;
        LateSimulationSystemGroup lateSimSys;
        protected override void OnCreate()
        {
            endSimEntComSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            lateSimSys = World.GetOrCreateSystem<LateSimulationSystemGroup>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer.ParallelWriter endCommandBuffer = endSimEntComSystem.CreateCommandBuffer().AsParallelWriter();

            Entities
                .ForEach((Entity entity, int entityInQueryIndex, ref LocalToWorld transform, ref PhysicsVelocity velocity, ref PhysicsMass mass,
                in ImpulseComponent impulse, in Translation translation, in Rotation rotation) =>
                {
                    PhysicsComponentExtensions.ApplyImpulse(ref velocity, in mass, in translation, in rotation, (-impulse.normal + new float3(0f, impulse.upwardMultiplier, 0f)) * impulse.force * 100, impulse.point);
                        float3 impulsePower = new float3(0f, 100f, 0f);
                        PhysicsComponentExtensions.ApplyAngularImpulse(ref velocity, in mass, in impulsePower);

                    //Impulse only once
                    endCommandBuffer.RemoveComponent<ImpulseComponent>(entityInQueryIndex, entity);
                }).ScheduleParallel();

            endSimEntComSystem.AddJobHandleForProducer(Dependency);
        }
    }
}