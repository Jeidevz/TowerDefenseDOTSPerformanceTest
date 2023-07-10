using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;


namespace TowerDefenseDOTS
{
    [Serializable]
    public struct PhysicsRotateTowardPosition : IComponentData
    {
        public float3 position;
        public float speed;
    }
}