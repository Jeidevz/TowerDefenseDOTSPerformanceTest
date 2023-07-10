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
    public class VFXSystem : SystemBase
    {
        BeginSimulationEntityCommandBufferSystem beginSimCommandBufferSys;

        protected override void OnCreate()
        {
            beginSimCommandBufferSys = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            //BUGGED. Sometimes missing data.
            //SOLVED! Reason was commandbuffer's batch destoy. Using EntityManager immediately destroy the batch fixed it weird behaviour.
            //------------------------------------------------------------------------------------------
            //EntityCommandBuffer beginCommandBuffer = beginSimCommandBufferSys.CreateCommandBuffer();
            //EntityQuery query = GetEntityQuery(ComponentType.ReadOnly<VFX>());
            //NativeArray<VFX> VFXs = query.ToComponentDataArray<VFX>(Allocator.TempJob);

            //foreach(VFX vfx in VFXs)
            //{
            //    GameObject go = GameObject.Instantiate(VFXManagerDOTS.effects[(int)vfx.effect], vfx.position, Quaternion.LookRotation(vfx.normal));
            //    if(vfx.parent != Entity.Null)
            //    {
            //        GOFollowEntity component = go.AddComponent<GOFollowEntity>();
            //        component.SetTarget(vfx.parent);
            //    }
            //}

            //beginCommandBuffer.DestroyEntity(query);

            //VFXs.Dispose();
            //beginSimCommandBufferSys.AddJobHandleForProducer(Dependency);
            //------------------------------------------------------------------------------------------

            Entities.ForEach((in VFX vfx) => {
                GameObject go = GameObject.Instantiate(VFXManagerDOTS.effects[(int)vfx.effect], vfx.position, Quaternion.LookRotation(vfx.normal));
                if (vfx.parent != Entity.Null)
                {
                    GOFollowEntity component = go.AddComponent<GOFollowEntity>();
                    component.SetTarget(vfx.parent);
                }

            }).WithoutBurst().WithStructuralChanges().Run();

            EntityQuery query = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<VFX>());
            EntityManager.DestroyEntity(query);
            //EntityCommandBuffer beginCommandBuffer = beginSimCommandBufferSys.CreateCommandBuffer();
            //beginCommandBuffer.DestroyEntity(query);
            //beginSimCommandBufferSys.AddJobHandleForProducer(Dependency);
        }
    }
}