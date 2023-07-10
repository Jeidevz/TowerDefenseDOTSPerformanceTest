using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct FollowPlayerComponent : IComponentData
    {
        public float distance;
        public float smooth;
    }
}