using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace TowerDefenseDOTS
{
    public class SpawnableRandomSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem beginSimCommandBuffer;
        Random random;
        protected override void OnCreate()
        {
            base.OnCreate();
            beginSimCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            random = new Random(2u);
        }

        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            EntityCommandBuffer.ParallelWriter ecbParallelWriter =  beginSimCommandBuffer.CreateCommandBuffer().AsParallelWriter();
            var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;

            Entities.ForEach((int entityInQueryIndex, int nativeThreadIndex, ref SpawnableRandom spawneableRandom, in Translation translation, in Rotation rotation) => {
                if (spawneableRandom.interval <= 0)
                {
                    var random = randomArray[nativeThreadIndex];
                    float3 randomPos;
                    GetRandomPosition(ref random, in spawneableRandom.areaSize, in translation.Value, out randomPos);
                    Translation positon = new Translation { Value = randomPos };
                    

                    Entity entity = ecbParallelWriter.Instantiate(entityInQueryIndex, spawneableRandom.entityPrefab);
                    ecbParallelWriter.SetComponent<Translation>(entityInQueryIndex, entity, new Translation { Value = randomPos });
                    ecbParallelWriter.SetComponent<Rotation>(entityInQueryIndex, entity, rotation);

                    spawneableRandom.interval = spawneableRandom.defaultInterval;
                    randomArray[nativeThreadIndex] = random;
                }

                spawneableRandom.interval -= dt;

            }).ScheduleParallel();

            beginSimCommandBuffer.AddJobHandleForProducer(Dependency);
        }


        public static void GetRandomPosition(ref Random random, in float2 areaSize, in float3 origin, out float3 randomPosition)
        {
            float3 offset = new float3(areaSize.x / 2, 0, areaSize.y / 2);
            float3 max = origin + offset;
            float3 min = origin - offset;
            randomPosition = random.NextFloat3(min, max);
        }
    }
}