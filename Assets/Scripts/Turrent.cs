using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Turrent : MonoBehaviour
    {
        

        [SerializeField] private TargetingSystem targetingSystem;
        [SerializeField] private MissileHolder missileHolder;
        [SerializeField] private TurrentGun[] turrentGuns;

        public void FireTurrentGuns()
        {
            for(int i = 0; i < turrentGuns.Length; i++)
            {
                turrentGuns[i].Fire();
            }
        }

        public bool ReloadMissile()
        {
            return missileHolder.LoadMissile();
        }

        public bool IsMissileAvailable()
        {
            return missileHolder.isMissileAvailable();
        }

        public bool IsMissileLoadedMax()
        {
            return missileHolder.IsFull();
        }

        public void FireMissile()
        {
            if (missileHolder.isMissileAvailable())
            {
                RaycastHit hit;
                if (targetingSystem.IsOnTarget(out hit))
                {
                    missileHolder.LaunchMissile(hit.point);
                }
            }
        }

    }
}
