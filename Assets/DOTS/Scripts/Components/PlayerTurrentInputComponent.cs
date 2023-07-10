using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TowerDefenseDOTS
{
    [GenerateAuthoringComponent]
    public struct PlayerTurrentInputComponent : IComponentData
    {
        public KeyCode fireGuns;
        public KeyCode fireMissile;
        public KeyCode moveLeft;
        public KeyCode moveRight;
        public KeyCode moveForward;
        public KeyCode moveBackward;
    }
}