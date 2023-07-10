using System;
using Unity.Entities;

namespace Testing
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct ExplosiveIntervalComponent : IComponentData
    {
        public ExplosionData explosionData;
        public float timer;
        public float defaultInterval;
    }
}


