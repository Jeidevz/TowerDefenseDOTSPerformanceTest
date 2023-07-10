using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHeight : MonoBehaviour
{
    public float height = 0;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }
}
