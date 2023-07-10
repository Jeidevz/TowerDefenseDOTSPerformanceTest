using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class EnemyRaycastHitReceiver : RaycastHitReceiver
    {
        [SerializeField] private EnemyCapsule enemyCapsule;

        public override void Hit(RaycastHitType hitType, in RaycastHit hitInfo)
        {
            //if(enemyCapsule)
            //    enemyCapsule.GotHit(hitInfo, 1000);
        }

        public override void Hit(RaycastHitType hitType, in RaycastHit hitInfo, float force)
        {
            if (enemyCapsule)
                enemyCapsule.GotHit(hitInfo, force);
        }
    }
}
