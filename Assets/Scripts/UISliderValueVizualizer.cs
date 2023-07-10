using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TowerDefense
{
    public class UISliderValueVizualizer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textGUI;
        [SerializeField] private Slider slider;

        public void ChangeSliderVisualValue(float value)
        {
            value = (float)System.Math.Round(value, 2);

            textGUI.text = value.ToString();
        }

        public void ChangeSliderValue(float value)
        {
            slider.value = value;
            ChangeSliderVisualValue(value);
        }

        public float GetSliderValue()
        {
            return slider.value;
        }
    }
}
