using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;


namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct LookDirectionVertically : IComponentData
    {
        public float3 direction;
    }
}
