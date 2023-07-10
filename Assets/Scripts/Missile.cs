using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Missile : MonoBehaviour
    {
        public static float MAX_ROTATION_SPEED = 100f;

        [SerializeField] private Rigidbody rg;
        [Header("Explosion")]
        [SerializeField] private Bomb bomb;
        [SerializeField] private float exploseRadius = 10f;
        [SerializeField] private float exploseForce = 1000f;
        [SerializeField] private float exploseUpModifier = 3f;
        [SerializeField] private float destroyDelayAfterExplosion = 3f;
        [Header("Exhaust")]
        [SerializeField] private Exhaust exhaust;
        [Space]
        [SerializeField] private float speed = 100f;
        [SerializeField] private float delayEngineStart = 1f;
        [SerializeField] private float lifetime = 20f;
        [SerializeField] private float targetReachDistance = 2f;
        [SerializeField] private float accuracyOverTime = 1f;
        [SerializeField] [Range(1f, 100f)] private float lookTargetRotateSpeed = 1f;
        [SerializeField] private bool explosiveOnCollision = false;
        [SerializeField] private Collider[] ownColliders;
        

        private Vector3 targetPosition;
        private float accuracy = 0;
        private bool engineOn = false;
        private bool targetReached = false;
        private bool exploded = false;

        // Update is called once per frame
        void Update()
        {
            if (engineOn)
            {
                if (!targetReached)
                {
                    RotateToTarget(lookTargetRotateSpeed);
                    CheckReachState();
                }

                MoveForward(speed);
            }
        }

        private void MoveForward(float speed)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void RotateToTarget(float rotateSpeed)
        {
            Vector3 targetDirection = (targetPosition - transform.position).normalized;
            float speed = rotateSpeed + accuracy;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), speed / 100);
            if(!IsAtMaxRotateSpeed())
                IncreaseRotateAccuracy(accuracyOverTime);
            
        }

        private bool IsHitByOwnCollider(in Collider other)
        {
            foreach(Collider col in ownColliders)
            {
                if (col == other)
                {
                    //Debug.Log("Hit by own colliders");
                    return true;

                }
            }

            //Debug.Log("Not hit by own colliders");
            return false;
        }

        private void IncreaseRotateAccuracy(float amount)
        {
            float totalSpeed = accuracy + lookTargetRotateSpeed;

            if (totalSpeed < 100)
                accuracy += amount * Time.deltaTime;
            else if (totalSpeed > 100)
                accuracy = 100 - lookTargetRotateSpeed;
        }

        private bool IsAtMaxRotateSpeed()
        {
            float totalSpeed = accuracy + lookTargetRotateSpeed;

            if (totalSpeed == MAX_ROTATION_SPEED)
                return true;
            else if (totalSpeed > 100)
            {
                accuracy = 100 - lookTargetRotateSpeed;
                return true;
            }

            return false;
        }

        private IEnumerator StartEngine(float delay)
        {
            yield return new WaitForSeconds(delay);

            engineOn = true;
            SetPhysics(false);
            TurnOnExhaust();
        }

        private void DetachPhysics(in Vector3 direction, float force)
        {
            rg.AddForce(direction * force, ForceMode.Force);
        }

        private void SetPhysics(bool state)
        {
            rg.useGravity = state;
            rg.isKinematic = !state;
        }

        private bool HasReachedToTarget()
        {
            return (transform.position - targetPosition).magnitude < targetReachDistance;
        }

        private void CheckReachState()
        {
            if (HasReachedToTarget())
                targetReached = true;
        }

        private void TurnOnExhaust()
        {
            exhaust.StartExhaust();
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Missile Hit: " + other.name);

            if (explosiveOnCollision && other.name != "Bomb" && !exploded /*&& !IsHitByOwnCollider(in other)*/)
            {
                DetachExhaust();
                Explode();
            }
        }

        private void DetachExhaust()
        {
            exhaust.transform.parent = null;
            exhaust.Shutdown();
            Destroy(exhaust.gameObject, 2f);
        }

        private void Explode()
        {
            bomb.gameObject.SetActive(true);
            bomb.Explode(transform.position);
            exploded = true;
            Destroy(gameObject, destroyDelayAfterExplosion);
        }

        private void SetExlosiveState(bool state)
        {
            explosiveOnCollision = state;
        }

        private void BlastPhysics(in Vector3 point)
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(point, exploseRadius);
            foreach (Collider hit in colliders)
            {
                //Debug.Log("Blast hits: " + hit.name);
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(exploseForce, point, exploseRadius, exploseUpModifier);
            }
        }

        public void LaunchMissile(in Vector3 targetPos, in Vector3 detachDirection, float force)
        {
            if(transform.parent)
                transform.parent = null;

            targetPosition = targetPos;
            SetExlosiveState(true);
            SetPhysics(true);
            DetachPhysics(detachDirection, force);
            StartCoroutine("StartEngine", delayEngineStart);
            Destroy(gameObject, lifetime);
        }

    }
}
