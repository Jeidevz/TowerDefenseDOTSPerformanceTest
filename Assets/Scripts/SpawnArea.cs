using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public Area area;

    public void calculateRandomLocation(out Vector3 location)
    {
        float minX = transform.position.x - area.width / 2;
        float maxX = minX + area.width;
        float minZ = transform.position.z - area.length / 2;
        float maxZ = minZ + area.length;
        float positionX = Random.Range(minX, maxX);
        float positionZ = Random.Range(minZ, maxZ);
        location = new Vector3(positionX, transform.position.y, positionZ);
    }

    private void OnDrawGizmos()
    {
        float height = 1;
        Gizmos.DrawWireCube(transform.position, new Vector3(area.width, height, area.length));
    }
}
