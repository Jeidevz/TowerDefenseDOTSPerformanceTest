using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class ChargeMeter : MonoBehaviour
    {
        private static float MAX_PERCENT = 100;

        [SerializeField] private float chargeSpeed;
        [SerializeField] private float totalPercent = 0;
        [Header("Meter")]
        [SerializeField] private RectTransform meter;
        [SerializeField] private float meterMaxHeight = 100f;

        private float currentPercent;

        private Coroutine chargeCoroutine;

        private void Start()
        {
            UpdateMeter(totalPercent);
        }

        private float CalculateSpeedMultiplier(float amountPercent, float speed)
        {
            float multiplier = amountPercent / speed;
            if (multiplier < 1)
                multiplier = 1;

            return multiplier;
        }
        private IEnumerator increasing(float amountPercent, float speed)
        {
            float multiplier = CalculateSpeedMultiplier(amountPercent, speed);

            while (currentPercent < totalPercent)
            {
                currentPercent += speed * Time.deltaTime * multiplier;
                UpdateMeter(currentPercent);
                yield return null;
            }

            currentPercent = totalPercent;
            UpdateMeter(totalPercent);
            yield return null;
        }

        private IEnumerator decreasing(float amountPercent, float speed)
        {
            float multiplier = CalculateSpeedMultiplier(amountPercent, speed);

            while (currentPercent > totalPercent)
            {
                currentPercent -= speed * Time.deltaTime * multiplier;
                UpdateMeter(currentPercent);
                yield return null;
            }

            currentPercent = totalPercent;
            UpdateMeter(totalPercent);
            yield return null;
        }

        private void StopChargeCoroutine()
        {
            if (chargeCoroutine != null)
                StopCoroutine(chargeCoroutine);
        }

        private void UpdateMeter(float valuePercent)
        {
            meter.sizeDelta = new Vector2(meter.rect.width, valuePercent / MAX_PERCENT * meterMaxHeight);
        }

        public void increase(float amountPercent)
        {
            StopChargeCoroutine();
            totalPercent = Mathf.Clamp(totalPercent + amountPercent, 0, MAX_PERCENT);
            chargeCoroutine = StartCoroutine(increasing(amountPercent, chargeSpeed));
        }

        public void decrease(float amountPercent)
        {
            StopChargeCoroutine();
            totalPercent = Mathf.Clamp(totalPercent - amountPercent, 0, MAX_PERCENT);
            chargeCoroutine = StartCoroutine(decreasing(amountPercent, chargeSpeed));
        }

        public void SetPercent(float value)
        {
            totalPercent = Mathf.Clamp(value, 0, MAX_PERCENT);
            UpdateMeter(totalPercent);
        }

        public float GetPercent()
        {
            return totalPercent;
        }

        public bool IsMaxed()
        {
            return totalPercent == MAX_PERCENT;
        }

        public void Reset()
        {
            totalPercent = 0;
            currentPercent = 0;
            UpdateMeter(currentPercent);
        }
    }
}