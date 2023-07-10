using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct Gun : IComponentData
    {
        public Entity bulletPrefab;
    }
}
