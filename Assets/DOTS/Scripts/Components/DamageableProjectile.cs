using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Authoring;


namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct DamageableProjectile : IComponentData
    {
        public PhysicsCategoryTags colliderBelongsTo;
        public PhysicsCategoryTags colliderCollidesWith;
        public float3 previousPosition;
        public float damage;
    }
}
