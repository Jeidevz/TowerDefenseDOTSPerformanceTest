using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private ParticleSystem explosionVFX;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private SphereCollider sensorCollider;
        [SerializeField] private float force = 1000f;
        [SerializeField] private float explodeAffectedRadius = 10f; 
        [SerializeField] private float explodeRadius = 20f; 
        [SerializeField] private float upModifier = 3f;
        [SerializeField] private float destroyTimer = 5f;
        [SerializeField] private bool activated = false;

        private void Awake()
        {
            sensorCollider.radius = explodeAffectedRadius;

            if (activated)
                Activate();
            else
                DisActivate();
        }

        private void Activate()
        {
            activated = true;
            sensorCollider.enabled = true;
        }

        private void DisActivate()
        {
            activated = false;
            sensorCollider.enabled = false;
        }

        private void ShowExplosionVFX()
        {
            explosionVFX.Play();
        }

        private void BlastPhysics(in Vector3 point)
        {
            float explosionRadius = sensorCollider.radius;

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(point, explosionRadius);
            foreach (Collider hit in colliders)
            {
                //Debug.Log("Blast hits: " + hit.name);
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(force, point, explosionRadius, upModifier);
            }
        }

        private void PlayExplosionSFX()
        {
            if(audioSource.enabled)
                audioSource.Play();
        }

        IEnumerator IEBlastPhysics(Vector3 point, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(point, explodeRadius);
            foreach (Collider hit in colliders)
            {
                //Debug.Log("Blast hits: " + hit.name);
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(force, point, explodeRadius, upModifier);
            }

            //sensorCollider.enabled = false;
            DisActivate();
        }

        public void Explode(in Vector3 point)
        {
            
            transform.parent = null;
            Activate();
            ShowExplosionVFX();
            PlayExplosionSFX();
            StartCoroutine(IEBlastPhysics(point, /*0.001f*/Time.deltaTime));
            Destroy(gameObject, destroyTimer);

            //Test shaking
            //TODO: Remove this after done testing
            CameraHolder.Shake(Camera.main, 1f, 1f, 0.5f, CameraHolder.ShakeType.Normal);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, explodeAffectedRadius);
        }

    }
}
