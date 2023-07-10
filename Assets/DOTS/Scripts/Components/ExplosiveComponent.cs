using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Authoring;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct ExplosiveComponent : IComponentData
    {
        public float damage;
        public float force;
        public float hitRadius;
        public float forceRadius;
        public float upForce;
        public float upModifier;
        public SoundLibraryScriptableObject.Clip clip;
        public PhysicsCategoryTags colliderBelongsTo;
        public PhysicsCategoryTags colliderCollidesWith;
    }
}