using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TowerDefense
{
    public class FPSCounterCustom : MonoBehaviour
    {
        public TextMeshProUGUI tmpGui;
        public TextMeshProUGUI tmpGuiUnscaled;

        void Update()
        {
            float fps = 1 / Time.deltaTime;
            float unscaledfps = 1 / Time.unscaledDeltaTime;
            tmpGui.SetText("FPS: " + fps);
            tmpGuiUnscaled.SetText("US FPS: " + unscaledfps);
        }
    }
}
