using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Testing
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct LifetimeComponent : IComponentData
    {
        public float time;
    }
}