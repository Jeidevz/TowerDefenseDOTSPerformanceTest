using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct ExplodedArea : IComponentData
    {
        public ExplosiveComponent explosive;
        public float3 point;
    }
}