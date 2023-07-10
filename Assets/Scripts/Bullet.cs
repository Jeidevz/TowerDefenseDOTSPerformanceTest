using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Bullet : MonoBehaviour
    {
        public GameObject impactVFX;
        public float lifetime = 10f;
        public float speed = 100;
        public float force = 200000;

        private Vector3 previousPos;

        void Start()
        {
            previousPos = transform.position;
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            previousPos = transform.position;

            transform.position += transform.forward * speed * Time.deltaTime;
            RaycastHit hit;
            if(isHit(out hit))
            {
                RaycastHitReceiver rayHitReceiver = hit.collider.GetComponent<RaycastHitReceiver>();
                if (rayHitReceiver)
                    rayHitReceiver.Hit(RaycastHitType.Bullet, in hit, force);

                Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Vector3 pos = hit.point;
                AddImpactVFX(pos, rot);
                Destroy(gameObject);
            }

        }

        private void AddImpactVFX(in Vector3 position, in Quaternion rotation)
        {
            Vector3 addedOffsetPos = position + impactVFX.transform.position;
            Quaternion VFXOffsetRot = impactVFX.transform.rotation;
            Quaternion addedOffsetRot = rotation * Quaternion.Euler(VFXOffsetRot.eulerAngles);
            Instantiate(impactVFX, position, addedOffsetRot);
        }

        private bool isHit(out RaycastHit hit)
        {
            return Physics.Raycast(previousPos, (transform.position - previousPos).normalized, out hit, (transform.position - previousPos).magnitude);
        }
    }
}
