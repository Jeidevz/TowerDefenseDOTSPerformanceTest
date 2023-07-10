using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class TurrentGun : MonoBehaviour
    {
        [SerializeField] private Transform aimer;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float smoothing = 0f;
        [SerializeField] private GunGeneral[] guns;

        void Start()
        {

        }

        void Update()
        {
            if (aimer)
                Aim();
        }

        private void Aim()
        {
            Vector3 direction = new Vector3(transform.forward.x, aimer.forward.y, transform.forward.z);
            //Vector3 direction = new Vector3(holder.forward.x, transform.forward.y, transform.forward.z);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1 - smoothing);
        }

        public void Fire()
        {
            for(int i = 0; i < guns.Length; i++)
            {
                guns[i].Fire();
            }
        }


    }
}
