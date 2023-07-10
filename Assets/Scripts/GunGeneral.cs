using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class GunGeneral : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform posBarrelHead;
        [SerializeField] private float intervalFire = .5f;
        [SerializeField] private ParticleSystem muzzleFlash;
        [Space]
        [SerializeField] private AudioSource audioSrc;
        [SerializeField] private AudioClip fireClip;

        private float intervalTimer = 0;

        private void Start()
        {
            audioSrc.clip = fireClip;
        }

        //Test firing
        private void Update()
        {
            if (intervalTimer > 0)
                intervalTimer -= Time.deltaTime;
        }

        private void InstantiateBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab, 
                posBarrelHead.position, posBarrelHead.rotation * Quaternion.Euler(bulletPrefab.transform.eulerAngles));
            //Rigidbody rg = bullet.GetComponent<Rigidbody>();
            //rg.AddForce(posBarrelHead.transform.forward * forceFire);
        }

        private bool CheckAbleToShoot()
        {
            return intervalTimer <= 0;
        }

        private void ResetTimer()
        {
            intervalTimer = intervalFire;
        }

        private void PlayFireSFX()
        {
            if(audioSrc.enabled)
                audioSrc.Play();
        }

        private void ShowMuzzleFlash()
        {
            muzzleFlash.Play();
        }

        public void Fire()
        {
            if (CheckAbleToShoot())
            {
                InstantiateBullet();
                ResetTimer();
                PlayFireSFX();
                ShowMuzzleFlash();
            }

        }
    }
}   
