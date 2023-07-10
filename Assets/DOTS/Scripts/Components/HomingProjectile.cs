using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;


namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct HomingProjectile : IComponentData
    {
        public float3 targetPosition;
        public float delay;
        public float speed;
        public float stopHomingDistance;
    }
}