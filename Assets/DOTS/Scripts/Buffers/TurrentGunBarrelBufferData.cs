using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [InternalBufferCapacity(4)][Serializable]
    public struct TurrentGunBarrelBufferData : IBufferElementData
    {
        public Entity barrel;
    }
}