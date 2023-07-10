using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class SpawnByInput : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] Transform spawnPlace;
        [SerializeField] KeyCode inputKey;

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(inputKey))
            {
                Spawn(ref prefab, spawnPlace.position, spawnPlace.rotation);
            }
        }

        private void Spawn(ref GameObject prefab, in Vector3 position, in Quaternion rotation)
        {
            Instantiate(prefab, position, rotation);
        }
    }
}
