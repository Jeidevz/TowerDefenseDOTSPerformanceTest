using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct ExplosionForced : IComponentData
    {
        public float3 point;
        public float force;
        public float radius;
        public float upForce;
        public float upwardsModifier;

    }
}