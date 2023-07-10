using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;



namespace TowerDefenseDOTS
{
    public class EnitySpawnerGO : MonoBehaviour
    {
        [System.Serializable]
        public struct Size
        {
            public float width;
            public float lenght;
        }

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
        [SerializeField] int amount = 100;
        [SerializeField] float areaWidth = 100f;
        [SerializeField] float spacing = 1f;
        [SerializeField] bool maintainAmountOfSpawns;

        GameObjectConversionSettings settings;

        // Start is called before the first frame update
        void Start()
        {
            if (amount < 1)
                return;

            SpawnBatch();

        }

        private void SpawnRandomLocationInLine(Entity entityPrefab, ref EntityManager entityManager)
        {
            Vector3 location;
            CalculateRandomLocationInLine(out location);
            Spawn(entityPrefab, in location, transform.forward, ref entityManager);
        }
        private void CalculateRandomLocationInLine(out Vector3 location)
        {
            float minX = transform.position.x - areaWidth / 2;
            float maxX = minX + areaWidth;
            float positionX = Random.Range(minX, maxX);
            location = new Vector3(positionX, transform.position.y, transform.position.z);
        }

        private void SpawnBatch()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            Entity entityPrefab = ConvertGOPrefabToEntity(in prefab, ref entityManager);

            Grid grid;
            CalculateGrid(areaWidth, spacing, amount, out grid);
            Offset offset;
            CalculateOffset(in grid, transform, areaWidth, spacing, out offset);

            for (int i = 0; i < amount; i++)
            {
                Vector3 spawnLocation = new Vector3();
                CalculateNextAvailablePosition(ref spawnLocation, transform, in grid, in offset, areaWidth, spacing, i);
                Spawn(entityPrefab, in spawnLocation, transform.forward, ref entityManager);
            }
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

        private void OnDrawGizmos()
        {
            float yAxisOffset = 2f;

            Grid grid;
            CalculateGrid(areaWidth, spacing, amount, out grid);

            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.up * yAxisOffset / 2, new Vector3((grid.column - 1) * spacing, yAxisOffset, (grid.row - 1) * spacing));

        }

        Entity ConvertGOPrefabToEntity(in GameObject prefab, ref EntityManager entityManager)
        {
            settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());
            Entity entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, settings);
            return entityPrefab;

        }

        public void Spawn(Entity entityPrefab, in Vector3 location, Vector3 watchDirection, ref EntityManager entityManager)
        {
            Entity spawnEntity = entityManager.Instantiate(entityPrefab);
            entityManager.SetComponentData<Translation>(spawnEntity, new Translation { Value = location });
            entityManager.SetComponentData<Rotation>(spawnEntity, new Rotation { Value = Quaternion.LookRotation(watchDirection) });
        }

        private void OnDestroy()
        {
            settings.BlobAssetStore.Dispose();
        }
    }
}