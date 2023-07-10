using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine.Diagnostics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct Fireable : IComponentData
    {
        public Entity ammoPrefab;
        public Lifetime ammoLifeTime;
        public PhysicsCategoryTags ammoBelongsTo;
        public PhysicsCategoryTags ammoCollidesWith;
        public FirePower firePower;
        public FireRate fireRate;
        public float timeLeftTillNextFire;
        public SoundLibraryScriptableObject.Clip clip;
    }
}
