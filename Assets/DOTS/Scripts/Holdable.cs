using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [Serializable]
    public struct Holdable : IComponentData
    {
        public Entity itemPrefab;
        public float distanceBetween;
        public float3 holdForwardDirection;
        public float itemPositionUpdateDelay;
    }
}