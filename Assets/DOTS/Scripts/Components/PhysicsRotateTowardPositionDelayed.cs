using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [Serializable]
    public struct PhysicsRotateTowardPositionDelayed : IComponentData
    {
        public PhysicsRotateTowardPosition physicsRotate;
        public float delay;

    }
}
