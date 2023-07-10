using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [Serializable]
    public struct GunBarrelsBuffer : IBufferElementData
    {
        Entity entity;
    }
}