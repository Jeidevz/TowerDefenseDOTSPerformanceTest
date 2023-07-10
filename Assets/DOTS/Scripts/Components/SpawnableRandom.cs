using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct SpawnableRandom : IComponentData
    {
        public Entity entityPrefab;
        public float2 areaSize;
        public float interval;
        public float defaultInterval;
    }
}
