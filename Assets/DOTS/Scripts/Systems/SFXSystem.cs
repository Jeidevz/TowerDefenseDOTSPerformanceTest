using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace TowerDefenseDOTS
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class SFXSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem beginSimCommandBufferSys;

        protected override void OnCreate()
        {
            beginSimCommandBufferSys = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }
        protected override void OnUpdate()
        {
            //BUGGED! Sometimes missing data.
            //SOLVED! Reason was commandbuffer's batch destoy. Using EntityManager immediately destroy the batch fixed it weird behaviour.
            //-------------------------------------------------------
            //EntityCommandBuffer beginCommandBuffer = beginSimCommandBufferSys.CreateCommandBuffer();
            //EntityQuery query = GetEntityQuery(ComponentType.ReadOnly<SFX>());
            //NativeArray<SFX> sfxs = query.ToComponentDataArray<SFX>(Allocator.TempJob);

            //foreach(SFX sfx in sfxs)
            //{
            //    if (sfx.parent != Entity.Null)
            //    {
            //        GameObject go = new GameObject();
            //        GOFollowEntity component = go.AddComponent<GOFollowEntity>();
            //        component.SetTarget(sfx.parent);
            //        AudioSource audioSource = go.AddComponent<AudioSource>();
            //        audioSource.volume = .5f;
            //        audioSource.PlayOneShot(SoundManager.SfxClips[(int)sfx.clip]);
            //        GameObject.Destroy(go, 1f);
            //    }
            //    else
            //        AudioSource.PlayClipAtPoint(SoundManager.SfxClips[(int)sfx.clip], sfx.position, 1f);
            //}

            //beginCommandBuffer.DestroyEntity(query);

            //sfxs.Dispose();
            //beginSimCommandBufferSys.AddJobHandleForProducer(Dependency);
            //-------------------------------------------------------

            Entities.ForEach((in SFX sfx) => {
                if (sfx.parent != Entity.Null)
                {
                    GameObject go = new GameObject();
                    GOFollowEntity component = go.AddComponent<GOFollowEntity>();
                    component.SetTarget(sfx.parent);
                    AudioSource audioSource = go.AddComponent<AudioSource>();
                    audioSource.volume = .5f;
                    audioSource.PlayOneShot(SoundManager.SfxClips[(int)sfx.clip]);
                    GameObject.Destroy(go, 1f);
                }
                else
                    AudioSource.PlayClipAtPoint(SoundManager.SfxClips[(int)sfx.clip], sfx.position, 1f);

            }).WithoutBurst().WithStructuralChanges().Run();

            EntityQuery query = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<SFX>());
            EntityManager.DestroyEntity(query);
            //EntityCommandBuffer beginCommandBuffer = beginSimCommandBufferSys.CreateCommandBuffer();
            //beginCommandBuffer.DestroyEntity(query);
            //beginSimCommandBufferSys.AddJobHandleForProducer(Dependency);
        }
    }
}
