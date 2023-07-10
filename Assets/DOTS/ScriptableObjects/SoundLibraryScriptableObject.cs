using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*IMPORTANT!
 * To make this work with DOTS and play correct clip is to assign audioclips to correct array position.
 * 0 = Default, Is for placeholder if not sound clip is assigned.
 * 1 = Bullet fire
 * 2 = Bullet hit
 * 3 = Enemy hit scream
 */ 

namespace TowerDefenseDOTS
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoundLibraryScriptableObject", order = 1)]
    public class SoundLibraryScriptableObject : ScriptableObject
    {
        public enum Clip
        {
            Bullet,
            BulletHit,
            MissileExplosion,
            Exhaust,
            EnemyHitScream
        }

        public AudioClip[] clips;
    }
}