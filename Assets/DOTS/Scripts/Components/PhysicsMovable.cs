using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [Serializable]
    public struct PhysicsMovable : IComponentData
    {
        public float3 direction;
        public float speed;
    }
}