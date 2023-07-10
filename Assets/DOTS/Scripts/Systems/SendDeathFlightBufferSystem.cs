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
    [DisableAutoCreation]
    [UpdateAfter(typeof(BulletRaycastSystem))]
    public class SendDeathFlightBufferSystem : SystemBase
    {
        DynamicBuffer<SendDeathFlightBufferData> bufferData;
        protected override void OnCreate()
        {
            base.OnCreate();
            //Entity entity = EntityManager.CreateEntity();
            //bufferData = EntityManager.AddBuffer<SendDeathFlightBufferData>(entity);
            //EntityManager.Instantiate(entity);
        }

        protected override void OnUpdate()
        {

            EntityQuery bufferQuery = GetEntityQuery(typeof(SendDeathFlightBufferData));
            NativeArray<Entity> bufferEntities = bufferQuery.ToEntityArray(Allocator.TempJob);
            BufferFromEntity<SendDeathFlightBufferData> buffer = GetBufferFromEntity<SendDeathFlightBufferData>();


            foreach (SendDeathFlightBufferData data in buffer[bufferEntities[0]])
            {
                if (EntityManager.Exists(data.entity) && !EntityManager.HasComponent<Lifetime>(data.entity))
                {
                    PhysicsMass entityMass = EntityManager.GetComponentData<PhysicsMass>(data.entity);
                    entityMass.InverseMass = 60f;
                    entityMass.InverseInertia = 9.81f;
                    PhysicsGravityFactor gravityFactor = EntityManager.GetComponentData<PhysicsGravityFactor>(data.entity);
                    gravityFactor.Value = 9.81f;
                    EntityManager.AddComponentData<Lifetime>(data.entity, new Lifetime { value = 1f });
                    EntityManager.RemoveComponent<ForwardMovable>(data.entity);
                    EntityManager.SetComponentData<PhysicsVelocity>(data.entity, new PhysicsVelocity
                    {
                        Linear = (-data.normal + new float3(0, 1, 0)) * 50,
                        Angular = new float3(-10f, 0f, 0f)
                    });
                }
            }

            buffer[bufferEntities[0]].Clear();
            bufferEntities.Dispose();
        }
    }
}