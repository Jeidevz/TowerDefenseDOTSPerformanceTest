using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Authoring;
using Unity.Rendering;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct Ammo : IComponentData
    {
        public Entity ammoPrefab;
        public Lifetime lifetime;
        public PhysicsCategoryTags colliderBelongsTo;
        public PhysicsCategoryTags colliderCollidesWith;
    }
}