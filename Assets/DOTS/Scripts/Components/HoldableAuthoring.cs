using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TowerDefenseDOTS
{
    public class HoldableAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {

        public GameObject itemPrefab;
        public int amount = 4;
        public float spaceBetween = 2f;
        public float3 itemForwardHoldDirection;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            Entity prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(itemPrefab,
                GameObjectConversionSettings.FromWorld(dstManager.World, conversionSystem.BlobAssetStore));

            dstManager.AddComponentData<Holdable>(entity, new Holdable
            {
                itemPrefab = prefabEntity,
                distanceBetween = spaceBetween,
                holdForwardDirection = itemForwardHoldDirection,
                itemPositionUpdateDelay = 5f
            });

            DynamicBuffer<EntityBufferElement> entitiesBuffer = dstManager.AddBuffer<EntityBufferElement>(entity);
            EntityCommandBuffer commandBuffer = dstManager.World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
            for (int i = 0; i < amount; i++)
            {
                Entity newItem = dstManager.Instantiate(prefabEntity);
                commandBuffer.AddComponent<Parent>(newItem, new Parent { Value = entity });
                float3 normDir = math.normalizesafe(itemForwardHoldDirection);
                quaternion forDirRot = Quaternion.LookRotation(normDir, math.up());
                commandBuffer.AddComponent<LocalToParent>(newItem, new LocalToParent { Value = float4x4.zero});
                commandBuffer.SetComponent<Rotation>(newItem, new Rotation { Value = forDirRot });
                commandBuffer.AppendToBuffer<EntityBufferElement>(entity, new EntityBufferElement { entity = newItem });
            }


        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(itemPrefab);
        }
    }
}