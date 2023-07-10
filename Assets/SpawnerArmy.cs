using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefenseDOTS
{
    public class SpawnerArmy : MonoBehaviour
    {
        public struct Grid
        {
            public int row;
            public int column;
        }

        public struct Offset
        {
            public Vector3 right;
            public Vector3 forward;
        }

        [SerializeField] GameObject prefab;
        [SerializeField] Transform container;
        [SerializeField] int amount = 1000;
        [SerializeField] float spacing = 1f;
        [SerializeField] float areaWidth = 100f;
        [SerializeField] bool maintainAmountOfSpawns = false;
        [SerializeField] KeyCode armySpawnKey;

        // Start is called before the first frame update
        void Start()
        {

            if (amount < 1)
                return;

            SpawnBatch(in prefab, amount);
        }

        private void Update()
        {
            if (maintainAmountOfSpawns)
            {
                int missingAmount = amount - container.childCount;
                if (missingAmount > 0)
                {
                    for (int i = 0; i < missingAmount; i++)
                    {
                        SpawnRandomLocationInLine(in prefab);
                    }
                }
            }

            if (Input.GetKeyDown(armySpawnKey))
            {
                SpawnBatch(in prefab, amount);
            }
        }

        private void SpawnBatch(in GameObject prefab, int amount)
        {
            Grid grid;
            CalculateGrid(areaWidth, spacing, amount, out grid);
            Offset offset;
            CalculateOffset(in grid, transform, areaWidth, spacing, out offset);

            for (int i = 0; i < amount; i++)
            {
                Vector3 spawnLocation = new Vector3();
                CalculateNextAvailablePosition(ref spawnLocation, transform, in grid, in offset, areaWidth, spacing, i);
                Spawn(in prefab, in spawnLocation, transform.forward, ref container);
            }
        }

        public void Spawn(in GameObject prefab, in Vector3 location, in Vector3 watchDirection, ref Transform container)
        {
            GameObject go;
            go = Instantiate(prefab, location, Quaternion.LookRotation(watchDirection));

            if (container)
                go.transform.parent = container;
        }

        private void CalculateNextAvailablePosition(ref Vector3 spawnLocation, Transform transform, in Grid grid, in Offset offset, float areaWidth, float spacing, int spawned)
        {
            int columnNumber = 0;

            if (spawned >= grid.column)
                columnNumber = spawned % grid.column;
            else
                columnNumber = spawned;

            int rowNumber = spawned / grid.column;

            //Debug.Log("Placement row: " + rowNumber + " column: " + columnNumber);

            spawnLocation = transform.position + (-transform.right * spacing * columnNumber) + (-transform.forward * spacing * rowNumber);
            spawnLocation += offset.forward + offset.right;
        }

        private void CalculateGrid(float areaWidth, float spacing, int totalSpawnAmount, out Grid outGrid)
        {
            outGrid = new Grid();
            outGrid.column = (int)(areaWidth / spacing);
            outGrid.row = totalSpawnAmount / outGrid.column;
        }

        private void CalculateOffset(in Grid grid, in Transform transform, float areaWidth, float spacing, out Offset outOffset)
        {
            outOffset = new Offset();
            outOffset.right = transform.right * (areaWidth / 2);
            outOffset.forward = transform.forward * grid.row * spacing / 2;
        }

        private void SpawnRandomLocationInLine(in GameObject prefab)
        {
            Vector3 location;
            CalculateRandomLocationInLine(out location);
            Spawn(in prefab, in location, transform.forward, ref container);
        }

        private void CalculateRandomLocationInLine(out Vector3 location)
        {
            float minX = transform.position.x - areaWidth / 2;
            float maxX = minX + areaWidth;
            float positionX = Random.Range(minX, maxX);
            location = new Vector3(positionX, transform.position.y, transform.position.z);
        }

        private void OnDrawGizmos()
        {

            float yAxisOffset = 2f;

            Grid grid;
            CalculateGrid(areaWidth, spacing, amount, out grid);

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.up * yAxisOffset / 2, new Vector3((grid.column - 1) * spacing, yAxisOffset, (grid.row - 1) * spacing));
        }
    }
}