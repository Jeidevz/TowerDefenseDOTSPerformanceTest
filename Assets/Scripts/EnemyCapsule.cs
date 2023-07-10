using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class EnemyCapsule : MonoBehaviour
    {
        public delegate void OnDeath();
        public Transform parent;
        public Rigidbody rg;
        public Animator anim;
        public List<string> tagsHittable;

        private static OnDeath onDeathDelegate;

        [Header("Physics")]
        [SerializeField] private float upwardModifier = 0;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> hurtSounds;

        [Header("Dead setting")]
        [SerializeField] private float destroyDelay = 2f;
        [SerializeField] private float deathScore = 100f;

        [SerializeField ]private bool isDead = false;

        private bool IsTagInList(in string tag)
        {
            return tagsHittable.Contains(tag);
        }

        private void Die()
        {
            isDead = true;
            InvokeDeathDelegate();
            ExecuteDestroy();
        }

        private void InvokeDeathDelegate()
        {
            if (onDeathDelegate != null)
                onDeathDelegate.Invoke();
        }

        private void PhysicsEnabled()
        {
            rg.isKinematic = false;
            rg.useGravity = true;
        }

        private void DisableMovement()
        {
            parent.GetComponent<MoveDirection>().enabled = false;
            if(anim)
                anim.enabled = false;
        }

        private void ExecuteDestroy()
        {
            if (parent)
                Destroy(parent.gameObject, destroyDelay);
            else
                Destroy(gameObject, destroyDelay);
        }

        private void DoHitPhysics(in RaycastHit hit, float force)
        {
            rg.AddForceAtPosition((-hit.normal + Vector3.up * upwardModifier).normalized * force, hit.point);
            //rg.AddExplosionForce(200000, hit.point, 20, .5f);
        }

        private void PlayHurtSound()
        {
            if (!audioSource)
                return;

            AudioClip randomClip = hurtSounds[Random.Range(0, hurtSounds.Count - 1)];
            audioSource.clip = randomClip;
            audioSource.Play();
        }

        private void AddScore()
        {
            GameManager.AddScore(deathScore);
        }

        private void LimpDead()
        {
            AddScore();
            DisableMovement();
            PhysicsEnabled();
            PlayHurtSound();
            Die();
        }

        public void GotHit(in RaycastHit hit, float force)
        {
            if (!isDead)
            {
                AddScore();
                DisableMovement();
                PhysicsEnabled();
                DoHitPhysics(in hit, force);
                PlayHurtSound();
                Die();
            }
            else
                DoHitPhysics(in hit, force);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Bomb")
                Debug.Log("Got hit by explosion. Name: " + collision.gameObject.name);
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log(gameObject.name + "got hit by " + other.name);
            if(other.name == "Bomb")
            {
                if(!isDead)
                    LimpDead();
            }

        }

        public static void AddDeathListener(in OnDeath listener)
        {
            onDeathDelegate += listener;
        }
        public static void RemoveDeathListener(in OnDeath listener)
        {
            onDeathDelegate -= listener;
        }

        public static void ResetDelegates()
        {
            onDeathDelegate = null;
        }

        

    }
}
