using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefenseSim
{

    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform target;

        [Header("Rotation")]
        public bool rotatable = true;
        public float verticalRotateSpeed = 100f;
        public float horizontalRotateSpeed = 100f;
        public float distanceFromTarget = 2f;
        public float rightFromTarget = 0f;
        public float upFromtTarget = 0f;
        public int rotateVMin = -60, rotateVMax = 80;
        public bool limitRotateHorizontal = false;
        public float rotateHMin = -65f, rotateHMax = 65f;
        public float rotationSmoothTime = 0.1f;
        public float rotateSensitivity = 1;

        [Header("Zoom")]
        public bool zoomable = true;
        public float zoomSpeed = 1f;
        public float zoomMin = 0;
        public float zoomMax = 10;

        Vector3 rotationSmoothVelocity;
        Vector3 currentRotation;
        float rotateV, rotateH;

        void LateUpdate()
        {
            if (zoomable)
            {
                distanceFromTarget += -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
                if (distanceFromTarget > zoomMax)
                    distanceFromTarget = zoomMax;
                else if (distanceFromTarget < 0)
                    distanceFromTarget = zoomMin;
            }

            if (rotatable)
            {
                rotateH += Input.GetAxisRaw("Mouse X") * horizontalRotateSpeed * rotateSensitivity * Time.deltaTime;
                if(limitRotateHorizontal)
                    rotateH = Mathf.Clamp(rotateH, rotateHMin, rotateHMax);

                rotateV -= Input.GetAxisRaw("Mouse Y") * verticalRotateSpeed * rotateSensitivity * Time.deltaTime;
                rotateV = Mathf.Clamp(rotateV, rotateVMin, rotateVMax);


                Vector3 targetRotation = new Vector3(rotateV, rotateH);

                currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

                transform.eulerAngles = currentRotation;
            }

            transform.position = target.position - transform.forward * distanceFromTarget + transform.up * upFromtTarget + transform.right * rightFromTarget;
        }

        public void SetZoomableState(bool state)
        {
            zoomable = state;
        }

        public void SetRotatable(bool state)
        {
            rotatable = state;
        }

        public void SetRotateSensitivity(float value)
        {
            rotateSensitivity = value;
        }
    }
}
