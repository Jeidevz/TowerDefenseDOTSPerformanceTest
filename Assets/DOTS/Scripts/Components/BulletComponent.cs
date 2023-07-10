using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct BulletComponent : IComponentData
    {
        public float damage;
        public PhysicsCategoryTags colliderBelongsTo;
        public PhysicsCategoryTags colliderCollidesWith;
        public float3 previousPosition;
    }
}
