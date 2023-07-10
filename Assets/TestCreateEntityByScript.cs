using UnityEngine;
using Unity.Entities;

namespace Testing
{

    public class TestCreateEntityByScript : MonoBehaviour
    {
        public struct TestEntityComponent : IComponentData
        {
            public int data1;
            public float data2;
        }

        void Start()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity entity = entityManager.CreateEntity();
            entityManager.AddComponentData(entity, new TestEntityComponent
            {
                data1 = 1,
                data2 = 12.1f
            });
        }
    }
}
