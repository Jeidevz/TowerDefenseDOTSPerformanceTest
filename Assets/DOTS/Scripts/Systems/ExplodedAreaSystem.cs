using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace TowerDefenseDOTS
{
    [UpdateInGroup(typeof(PhysicChangesSystemGroup))]
    public class ExplodedAreaSystem : SystemBase
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
            CollisionWorld collisionWorld =  buildPhysicsWorld.PhysicsWorld.CollisionWorld;
            EntityCommandBuffer endSimCommandBuffer = endSimSys.CreateCommandBuffer();
            EntityCommandBuffer beginSimCommandBuffer = beginSimSys.CreateCommandBuffer();


            NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(100, Allocator.TempJob);
            JobHandle handle = Entities
                .ForEach((Entity entity, in ExplodedArea explodedAreaData) =>
                {
                    CollisionFilter filter = new CollisionFilter()
                    {
                            BelongsTo = explodedAreaData.explosive.colliderBelongsTo.Value,
                            CollidesWith = explodedAreaData.explosive.colliderCollidesWith.Value
                    };

                    BlobAssetReference<Collider> collider = SphereCollider.Create(new SphereGeometry
                    {
                        Center = float3.zero,
                        Radius = explodedAreaData.explosive.hitRadius
                    }, filter);

                    ColliderCastInput input = new ColliderCastInput
                    {
                        Collider = (Collider*)collider.GetUnsafePtr(),
                        Orientation = quaternion.identity,
                        Start = explodedAreaData.point,
                        End = explodedAreaData.point
                    };

                    if (collisionWorld.CastCollider(input, ref hits))
                    {
                        for (int i = 0; i < hits.Length; i++)
                        {
                            endSimCommandBuffer.AddComponent<ExplosionForced>(hits[i].Entity, new ExplosionForced
                            {
                                force = explodedAreaData.explosive.force,
                                point = explodedAreaData.point,
                                radius = explodedAreaData.explosive.forceRadius,
                                upForce = explodedAreaData.explosive.upForce,
                                upwardsModifier = explodedAreaData.explosive.upModifier
                            });

                            endSimCommandBuffer.AddComponent<Damaged>(hits[i].Entity, new Damaged { value = explodedAreaData.explosive.damage });
                        }
                    }
                    hits.Clear();

                    beginSimCommandBuffer.DestroyEntity(entity);
                }).Schedule(Dependency);

            handle.Complete();
            hits.Dispose();
            endSimSys.AddJobHandleForProducer(Dependency);
            beginSimSys.AddJobHandleForProducer(Dependency);
        }
    }
}