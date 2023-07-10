using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace Testing
{

    public class ObjectToEntityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public struct TestECSComponent : IComponentData
        {
            public int data;
        }
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new TestECSComponent
            {
                data = 1
            });

        }
    }
}
