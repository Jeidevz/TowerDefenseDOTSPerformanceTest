using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class TargetingSystem : MonoBehaviour
    {
        [SerializeField] private Transform looker;
        [SerializeField] private float lookDistance;

        public bool IsOnTarget(out RaycastHit outHit)
        {
            return Physics.Raycast(transform.position, transform.forward, out outHit, lookDistance);
        }
    }
}
