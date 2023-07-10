using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace TowerDefenseDOTS
{
    public class SpawnEntitiesGO : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] int amount;
        [SerializeField] SpawnArea spawnArea;

        int counter = 0;
        Entity entityPrefab;
        EntityManager entityManager;
        GameObjectConversionSettings settings;
        // Start is called before the first frame update
        void Start()
        {
            if (amount < 1)
                return;

            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());
            entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, settings);

            while (counter < amount)
            {
                spawnRandom(in spawnArea);
            }
        }

        public void spawn(Vector3 location, in Vector3 watchDirection)
        {
            Entity spawnEntity = entityManager.Instantiate(entityPrefab);
            entityManager.SetComponentData<Translation>(spawnEntity, new Translation { Value = location });
            entityManager.SetComponentData<Rotation>(spawnEntity, new Rotation { Value = Quaternion.LookRotation(watchDirection) });

            counter++;
        }

        public void spawnRandom(in SpawnArea area)
        {
            Vector3 location;
            area.calculateRandomLocation(out location);
            spawn(location, area.transform.forward);
        }

        private void OnDestroy()
        {
            settings.BlobAssetStore.Dispose();
        }
    }
}