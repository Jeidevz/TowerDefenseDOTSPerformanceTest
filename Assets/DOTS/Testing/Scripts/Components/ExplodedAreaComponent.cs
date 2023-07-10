using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Testing
{
    [Serializable]
    public struct ExplodedAreaComponent : IComponentData
    {
        public ExplosionData explosionData;
        public float3 position;
    }
}


