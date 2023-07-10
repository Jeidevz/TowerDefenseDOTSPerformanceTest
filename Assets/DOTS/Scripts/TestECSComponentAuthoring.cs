using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Testing
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class TestECSComponentAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        // GameObject variables
        [SerializeField] int _moveSpeed;
        [SerializeField] float _jumpHeight;

        //Test ECS Components
        //-----------------------------------------------------------------------------------------
        public struct TestMovableComponent : IComponentData
        {
            public float3 moveDirection;
            public int moveSpeed;
        }

        public struct TestJumpableComponent : IComponentData
        {
            public float jumpHeight;
        }
        //-----------------------------------------------------------------------------------------

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new TestMovableComponent {
                moveSpeed = _moveSpeed,
                moveDirection = transform.forward
            });

            dstManager.AddComponentData(entity, new TestJumpableComponent {
                jumpHeight = _jumpHeight
            });
        }
    }
}



