using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Authoring;
using UnityEngine;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct TurrentWeapons : IComponentData
    {
        //Lazy way but do it for now
        [Header("Arm Weapons")]
        public Entity leftArm;
        public Entity rightArm;
        public Entity leftBottomGunBarrel;
        public Entity rightBottomGunBarrel;
        public Entity ammoPrefab;
        public PhysicsCategoryTags ammoBelongsTo;
        public PhysicsCategoryTags ammoCollidesWith;
        [Header("Missile")]
        public Entity missileHolder;
        public Entity missilePrefab;

    }
}