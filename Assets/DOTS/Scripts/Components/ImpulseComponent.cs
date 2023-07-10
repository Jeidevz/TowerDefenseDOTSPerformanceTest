using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct ImpulseComponent : IComponentData
    {
        public float3 point;
        public float3 normal;
        public float force;
        public float upwardMultiplier;


    }
}
