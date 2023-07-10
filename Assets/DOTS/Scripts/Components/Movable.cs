using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct Movable : IComponentData
    {
        public float3 direction;
        public float speed;
    }
}
