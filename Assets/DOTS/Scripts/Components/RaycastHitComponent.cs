using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct RaycastHitComponent : IComponentData
    {
        public float3 hitPoint;
        public float3 normal;
        public float force;
    }
}