using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;


namespace TowerDefenseDOTS
{
    [Serializable]
    public struct MoveTargetLocalPosition : IComponentData
    {
        public float3 oldLocalPosition;
        public float3 targetLocalPosition;
        public float duration;
        public float speed;
    }
}