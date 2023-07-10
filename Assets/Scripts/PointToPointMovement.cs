using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class PointToPointMovement : MonoBehaviour
    {
        public Transform target;

        public enum Point
        {
            A,
            B
        }

        public Transform pointA;
        public Transform pointB;

        public void MoveToPoint(Point point, float speed)
        {
            Vector3 targetPos = (point == Point.A) ? pointA.position : pointB.position;

            target.position = Vector3.Lerp(target.position, targetPos, speed * Time.deltaTime);
        }

        //Test
        private void Update()
        {
            float speed = 2f;

            if (Input.GetKey(KeyCode.A))
                MoveToPoint(Point.A, speed);
            else if (Input.GetKey(KeyCode.D))
                MoveToPoint(Point.B, speed);

        }
    }
}
