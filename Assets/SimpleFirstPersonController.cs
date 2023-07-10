using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFirstPersonController : MonoBehaviour
{
    [SerializeField] Transform camera;
    [SerializeField] float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        transform.position += camera.forward * verticalInput * speed * Time.deltaTime;
        transform.position += camera.right * horizontalInput * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
            transform.position += Vector3.up * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
            transform.position += -Vector3.up * speed * Time.deltaTime;
    }
}
