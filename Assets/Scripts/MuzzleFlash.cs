using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class MuzzleFlash : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private Light flash;

        public void Flash()
        {
            particle.Play();
            flash.enabled = true;
        }
    }
}
