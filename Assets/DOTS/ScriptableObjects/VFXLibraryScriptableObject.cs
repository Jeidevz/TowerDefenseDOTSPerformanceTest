using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefenseDOTS
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/VFXLibraryScriptableObject", order = 1)]
    public class VFXLibraryScriptableObject : ScriptableObject
    {
        public enum Effect
        {
            BulletHit,
            MuzzleFlash,
            MissileExplosion
        }

        public GameObject[] effects;
    }
}