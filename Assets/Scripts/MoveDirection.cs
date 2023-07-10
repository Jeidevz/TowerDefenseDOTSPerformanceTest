using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class MoveDirection : MonoBehaviour
    {
        public Vector3 direction;
        public float speedMultiplier = 1f;
        public float randomSpeedMultiplierMin = 1f;
        public float randomSpeedMultiplierMax = 2f;
        public bool randomSpeed = false;

        private float randomMultiplier;

        private void Start()
        {
            randomMultiplier = Random.Range(randomSpeedMultiplierMin, randomSpeedMultiplierMax);
        }

        void Update()
        {
            Vector3 movement = direction * speedMultiplier * Time.deltaTime;
            if (randomSpeed)
                movement *= randomMultiplier;

            transform.position += movement;
        }
    }
}
