using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Testing
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct TestMovableComponent : IComponentData
    {
        public float3 moveDirection ;
        public int moveSpeed;
    }
}


