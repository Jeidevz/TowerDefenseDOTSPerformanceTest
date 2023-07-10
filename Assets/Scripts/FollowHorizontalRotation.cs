using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class FollowHorizontalRotation : MonoBehaviour
    {
        public Transform targetToFollow;
        public float smoothing = 1f;

        private Vector3 gizmoDirection;

        void Update()
        {
            Vector3 direction = new Vector3(targetToFollow.forward.x, transform.forward.y, targetToFollow.forward.z);
            //Debug.Log("Direction " + direction);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothing);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(targetToFollow.position, transform.position + targetToFollow.forward);
        }
    }
}