using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct SFX : IComponentData
    {
        public SoundLibraryScriptableObject.Clip clip;
        public float3 position;
        public Entity parent;
    }
}