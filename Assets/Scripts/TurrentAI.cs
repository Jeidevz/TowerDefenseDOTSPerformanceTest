using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;

namespace TowerDefense
{
    public class TurrentAI : MonoBehaviour
    {
        [SerializeField] private Turrent turrent;
        [SerializeField] private float loadMissileInterval = 10f;
        [SerializeField] private float fireMissileInterval = 5f;

        private float missileLoadTimer = 0;
        private float fireMissileTimer = 0;

        private void Update()
        {
            turrent.FireTurrentGuns();
            FireMissileAtInterval(fireMissileInterval);

            if(!turrent.IsMissileLoadedMax())
                LoadMissileAtInterval(loadMissileInterval);
        }

        private void LoadMissileAtInterval(float interval)
        {
            missileLoadTimer += Time.deltaTime;
            if(missileLoadTimer > interval)
            {
                turrent.ReloadMissile();
                missileLoadTimer = 0;
            }
        }

        private void FireMissileAtInterval(float interval)
        {
            fireMissileTimer += Time.deltaTime;

            if (turrent.IsMissileAvailable() && fireMissileTimer > interval)
            {
                turrent.FireMissile();
                fireMissileTimer = 0;
            }
        }




    }
}