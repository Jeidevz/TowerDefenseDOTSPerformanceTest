using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace performanceproject
{
    public class SpawnManager : MonoBehaviour
    {
        public enum SpawnMode
        {
            None,
            MaintainAmount,
            Continuously
        }
        
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform container;
        [SerializeField] private SpawnMode spawnMode = SpawnMode.None;
        [SerializeField] private float interval = 0;
        [SerializeField] private int amount = 10;
        [SerializeField] private bool spawnAllAtStart = true;
        [SerializeField] private bool randomPositionInArea = false;
        [SerializeField] private List<SpawnArea> spawnAreas;

        private float counter = 0;
        private int spawnedCounter = 0;

        void Start()
        {
            if (prefab && spawnAllAtStart)
                spawnAll(amount, randomPositionInArea);
        }

        //TODO: Optimize. It's garbage hell!
        private void Update()
        {
            if (!prefab || spawnMode == SpawnMode.None || (spawnMode == SpawnMode.MaintainAmount && container.childCount == amount))
                return;

            
            switch(spawnMode)
            {
                case SpawnMode.MaintainAmount:
                    SpawnWhileMaintainAmount();
                    break;
                case SpawnMode.Continuously:
                    SpawnContinuously();
                    break;
                default:
                    break;
            }
        }

        private void SpawnWhileMaintainAmount()
        {
            int headCount = container.childCount;

            int missingAmount = amount - headCount;

            if (interval == 0 && headCount < amount)
                spawnAll(missingAmount, randomPositionInArea);
            else if (interval > 0 && headCount < amount)
            {

                if (interval == 0)
                    spawnAll(missingAmount, randomPositionInArea);
                else if (interval > 0)
                {
                    counter += Time.deltaTime;
                    if (counter >= interval)
                    {
                        SpawnArea area = spawnAreas[Random.Range(0, spawnAreas.Count)];
                        if (randomPositionInArea)
                            spawnRandom(area);
                        else
                            spawn(area.transform.position, area.transform.forward);

                    }

                }

            }
        }

        private void SpawnContinuously()
        {
            counter += Time.deltaTime;
            if (counter < interval)
                return;

            SpawnArea area = spawnAreas[Random.Range(0, spawnAreas.Count)];
            if (randomPositionInArea)
                spawnRandom(area);
            else
                spawn(area.transform.position, area.transform.forward);
        }

        //Spawn center of the area.
        public void spawn(Vector3 location, in Vector3 watchDirection)
        {
            GameObject go = Instantiate(prefab, location, Quaternion.LookRotation(watchDirection));
            go.transform.parent = container;
            spawnedCounter++;
            counter = 0; // Reset counter;
        }

        //Spawn at random location in the area.
        public void spawnRandom(SpawnArea area)
        {
            Vector3 location;
            area.calculateRandomLocation(out location);
            spawn(location, area.transform.forward);
        }

        public void spawnAll(int amount, bool randomAreaPosition)
        {
            for (uint i = 0; i < amount; i++)
            {
                int number = Random.Range(0, spawnAreas.Count);
                if (randomAreaPosition)
                    spawnRandom(spawnAreas[number]);
                else
                    spawn(spawnAreas[number].transform.position, spawnAreas[number].transform.forward);
            }
        }

        public void setPrefab(GameObject newPrefab)
        {
            prefab = newPrefab;
        }

        public void manualRefreshSpawnCounter()
        {
            spawnedCounter = container.childCount;
        }

        public void SetAmount(uint value)
        {
            amount = (int)value;
        }

        public void SetInterval(float value)
        {
            interval = value;
        }
    }

}