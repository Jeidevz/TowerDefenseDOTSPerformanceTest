using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public class TestAudioComponent : IComponentData
    {
        // Add fields to your component here. Remember that:
        public AudioClip audioClip;
        public float3 position;
    }
}