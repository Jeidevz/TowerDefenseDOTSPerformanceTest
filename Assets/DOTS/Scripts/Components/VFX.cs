using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct VFX : IComponentData
    {

        public float3 position;
        public float3 normal;
        public VFXLibraryScriptableObject.Effect effect;
        public Entity parent;
    }
}