using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefenseDOTS
{
    public class LockFPS : MonoBehaviour
    {
        [SerializeField] bool lockFPS = false;
        [SerializeField] int fps = 60;
        // Start is called before the first frame update
        void Awake()
        {
            if (lockFPS)
            {
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = fps;
            }
        }

    }
}