using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace TowerDefenseDOTS
{

    public class CreateBuffersAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddBuffer<SendDeathFlightBufferData>(entity);
        }

        // Start is called before the first frame update
        void Awake()
        {
            //EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //Entity entity = entityManager.CreateEntity();
            //entityManager.AddBuffer<SendDeathFlightBufferData>(entity);
            //entityManager.Instantiate(entity);S
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}