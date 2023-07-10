using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct RotatingTowardTarget : IComponentData
    {
        public float3 targetPosition;
        public float speed;
        public float rotatedTime;
    }
}