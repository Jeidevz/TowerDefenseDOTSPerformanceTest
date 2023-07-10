using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;

namespace TowerDefense
{
    public class CameraHolder : MonoBehaviour
    {
        public enum ShakeType
        {
            Normal,
            UpDown,
            LeftRight,
            
        }

        [SerializeField] private Transform cam;

        private float verticalOffset = 0f;
        private float horizontalOffset = 0f;

        private Coroutine shakeCoroutine;
        private float timer = 0f;

        //Testing shake
        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.K))
        //        Shake(.5f, 2f, .5f, ShakeType.Normal);
        //}

        private IEnumerator NormalShake(float force, float intensity, float duration)
        {
            while(timer < duration)
            {
                SimpleHorizontalMovement(force, intensity);
                SimpleVerticalMovement(force, intensity);
                IncrementTimer();
                yield return null;
            }

            cam.localPosition = Vector3.zero;
            ResetTimer();
            yield return null;
        }

        private void SimpleVerticalMovement(float force, float intensity)
        {
            float randomOffset = Random.Range(-force, force);
            cam.position = Vector3.Lerp(transform.position, cam.position + cam.right * randomOffset, (1f + 1 * intensity) * Time.deltaTime);
        }
        private void SimpleHorizontalMovement(float force, float intensity)
        {
            float randomOffset = Random.Range(-force, force);
            cam.position = Vector3.Lerp(transform.position, cam.position + cam.up * randomOffset, (1f + 1 * intensity) * Time.deltaTime);
        }

        private void ResetTimer()
        {
            timer = 0;
        }

        private void IncrementTimer()
        {
            timer += Time.deltaTime;
        }

        public void Shake(float force, float intensity, float duration, ShakeType type)
        {
            if(shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);

            switch(type)
            {
                case ShakeType.Normal:
                    shakeCoroutine = StartCoroutine(NormalShake(force, intensity, duration));
                    break;
                case ShakeType.LeftRight:
                    break;
                case ShakeType.UpDown:
                    break;
                default:
                    break;
            }
        }

        public static bool Shake(in Camera mainCamera, float force, float intensity, float duration, ShakeType type)
        {
            CameraHolder holder = mainCamera.transform.parent.GetComponent<CameraHolder>();
            if (holder)
            {
                holder.Shake(force, intensity, duration, type);
                return true;
            }

            return false;
        }
    }
}