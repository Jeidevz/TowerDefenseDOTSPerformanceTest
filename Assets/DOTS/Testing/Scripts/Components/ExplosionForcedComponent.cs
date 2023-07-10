using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Testing
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct ExplosionForcedComponent : IComponentData
    {
        public float3 point;
        public float force;
        public float radius;
        public float upForce;
        public float upwardsModifier;
    }
}



