using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct Rotatable : IComponentData
    {
        public float3 rotate;
        public float speed;
    }
}