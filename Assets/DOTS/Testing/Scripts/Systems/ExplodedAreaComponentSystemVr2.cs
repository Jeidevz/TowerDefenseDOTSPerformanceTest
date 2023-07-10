using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace Testing
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(TowerDefenseDOTS.PhysicChangesSystemGroup))]
    public class ExplodedAreaComponentSystemVr2 : SystemBase
    {
        Unity.Physics.Systems.BuildPhysicsWorld buildPhysicsWorld;
        EndSimulationEntityCommandBufferSystem endSimSys;
        BeginSimulationEntityCommandBufferSystem beginSimSys;

        protected override void OnCreate()
        {
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            endSimSys = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            beginSimSys = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            CollisionWorld collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;
            EntityCommandBuffer.ParallelWriter endBuffer = endSimSys.CreateCommandBuffer().AsParallelWriter();
            EntityCommandBuffer.ParallelWriter beginBuffer = beginSimSys.CreateCommandBuffer().AsParallelWriter();

            EntityQueryDesc queryDesc = new EntityQueryDesc { 
                All = new ComponentType[] {ComponentType.ReadOnly<Tag_ExplosiveBall>(), ComponentType.ReadOnly<Translation>()},
             };

            EntityQuery query = GetEntityQuery(queryDesc);
            NativeArray<Entity> entities = query.ToEntityArray(Allocator.TempJob);
            NativeArray<Translation> translations = query.ToComponentDataArray<Translation>(Allocator.TempJob);

            Entities
               //.WithReadOnly(collisionWorld)
               .ForEach((Entity entity, in ExplodedAreaComponent explodedAreaComponent) =>
               {
                   for(int i = 0; i < translations.Length; i++)
                   {
                       float distance = math.distance(explodedAreaComponent.position, translations[i].Value);
                       if (distance < explodedAreaComponent.explosionData.radius)
                       {
                           endBuffer.AddComponent( i, entities[i], new ExplosionForcedComponent
                           {
                               force = explodedAreaComponent.explosionData.force,
                               point = explodedAreaComponent.position,
                               radius = explodedAreaComponent.explosionData.radius,
                               upForce = explodedAreaComponent.explosionData.upForce,
                               upwardsModifier = explodedAreaComponent.explosionData.upModifier
                           });
                       }
                   }
               }).WithDisposeOnCompletion(entities)
               .WithDisposeOnCompletion(translations)
               .ScheduleParallel();

            endSimSys.AddJobHandleForProducer(Dependency);
            beginSimSys.AddJobHandleForProducer(Dependency);
        }

    }
}