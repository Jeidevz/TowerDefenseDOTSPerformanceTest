using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace TowerDefenseDOTS
{
    public class GOFollowEntity : MonoBehaviour
    {
        private Entity targetEntity;
        private EntityManager manager;
        
        
        private void Awake()
        {
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            targetEntity = manager.CreateEntity();
            AddComponents(targetEntity, ref manager);
        }

        private void LateUpdate()
        {
            Translation translation = manager.GetComponentData<Translation>(targetEntity);
            Rotation rotation = manager.GetComponentData<Rotation>(targetEntity);
            LocalToWorld localToWorld = manager.GetComponentData<LocalToWorld>(targetEntity);
            transform.position = localToWorld.Position;
            transform.rotation = localToWorld.Rotation;
        }

        private void AddComponents(Entity entity, ref EntityManager dstManager)
        {
            manager.AddComponent<FollowPlayerComponent>(entity);
            manager.AddComponent<Translation>(entity);
            manager.AddComponent<Rotation>(entity);
            manager.AddComponent<LocalToWorld>(entity);
        }

        public void SetTarget(Entity target)
        {
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;

            if (targetEntity != Entity.Null)
                manager.DestroyEntity(targetEntity);

            targetEntity = target;
        }
    }
}
