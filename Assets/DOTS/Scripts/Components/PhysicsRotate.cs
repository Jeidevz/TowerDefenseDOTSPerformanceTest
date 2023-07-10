using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;


namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct PhysicsRotate : IComponentData
    {
        public float3 value;



    }
}