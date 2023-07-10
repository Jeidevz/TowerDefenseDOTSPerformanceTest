using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public abstract class RaycastHitReceiver : MonoBehaviour
    {
        public abstract void Hit(RaycastHitType hitType, in RaycastHit hitInfo);

        public abstract void Hit(RaycastHitType hitType, in RaycastHit hitInfo, float force);
    }
}