using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Audio;

namespace TowerDefenseDOTS
{
    public class TestAudioSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem endSimCommandSys;


        protected override void OnCreate()
        {
            base.OnCreate();
            endSimCommandSys = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer ecbParalWriter = endSimCommandSys.CreateCommandBuffer();


            Entities.ForEach((Entity entity, int entityInQueryIndex, ref Translation translation, in TestAudioComponent testAudio) =>
            {
                AudioSource.PlayClipAtPoint(testAudio.audioClip, testAudio.position);
                ecbParalWriter.RemoveComponent<TestAudioComponent>(entity);
                
            }).WithoutBurst().Run();
        }
    }
}