using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Testing
{
    public class ExplodedAreaComponentSystem : SystemBase
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

        protected unsafe override void OnUpdate()
        {
            CollisionWorld collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;
            EntityCommandBuffer endBuffer = endSimSys.CreateCommandBuffer();
            EntityCommandBuffer beginBuffer = beginSimSys.CreateCommandBuffer();

            NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(100, Allocator.TempJob);
             JobHandle handle = Entities
                .WithReadOnly(collisionWorld)
                .ForEach((Entity entity, in ExplodedAreaComponent explodedAreaComponent) =>
                {
                    CollisionFilter filter = new CollisionFilter()
                    {
                        BelongsTo = explodedAreaComponent.explosionData.belongsTo.Value,
                        CollidesWith = explodedAreaComponent.explosionData.collidesWith.Value
                    };

                    BlobAssetReference<Collider> collider = SphereCollider.Create(new SphereGeometry
                    {
                        Center = float3.zero,
                        Radius = explodedAreaComponent.explosionData.radius
                    }, filter);

                    ColliderCastInput input = new ColliderCastInput
                    {
                        Collider = (Collider*)collider.GetUnsafePtr(),
                        Orientation = quaternion.identity,
                        Start = explodedAreaComponent.position,
                        End = explodedAreaComponent.position
                    };

                    if (collisionWorld.CastCollider(input, ref hits))
                    {
                        for (int i = 0; i < hits.Length; i++)
                        {
                            endBuffer.AddComponent(hits[i].Entity, new ExplosionForcedComponent
                            {
                                force = explodedAreaComponent.explosionData.force,
                                point = explodedAreaComponent.position,
                                radius = explodedAreaComponent.explosionData.radius,
                                upForce = explodedAreaComponent.explosionData.upForce,
                                upwardsModifier = explodedAreaComponent.explosionData.upModifier
                            });
                        }
                    }
                    collider.Dispose();
                    hits.Clear();

                    beginBuffer.DestroyEntity(entity);
                }).WithDisposeOnCompletion(hits).Schedule(buildPhysicsWorld.GetOutputDependency());

            Dependency = handle;
            endSimSys.AddJobHandleForProducer(Dependency);
            beginSimSys.AddJobHandleForProducer(Dependency);
        }

    }
}