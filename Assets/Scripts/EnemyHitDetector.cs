using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class EnemyHitDetector : MonoBehaviour
    {
        public List<string> tagsHittable;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsTagInList(collision.transform.tag))
                Die();
        }

        private bool IsTagInList(in string tag)
        {
            return tagsHittable.Contains(tag);
        }

        private void Die()
        {
            if (transform.parent)
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject);
        }
    }
}
