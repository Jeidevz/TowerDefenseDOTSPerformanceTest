using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct SendDeathFlightComponent : IComponentData
    {
        public float3 impulse;
        public float3 point;
        public float3 normal;
        public float force;
        public float deathTimer;
    }
}