using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct RaycastEnemyHitComponent : IComponentData
    {
        public float3 oldPosition;
    }
}