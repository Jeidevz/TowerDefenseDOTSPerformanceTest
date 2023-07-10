using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Exhaust : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private AudioSource audioSource;

        private float defaultEmissionRate;

        void Start()
        {
            defaultEmissionRate = particles.emission.rateOverTime.constant;
        }

        public void StartExhaust()
        {
            //vfx.SetActive(true);
            particles.Play();
            audioSource.Play();
        }

        public void Shutdown()
        {
            //vfx.SetActive(false);
            //particles.loop = false;
            
            particles.Stop();
            audioSource.Stop();
        }

    }
}
