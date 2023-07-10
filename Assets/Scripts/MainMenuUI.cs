using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject window;
        public void Open()
        {
            window.SetActive(true);
        }

        public void Close()
        {
            window.SetActive(false);
        }

        public bool IsOpen()
        {
            return window.activeSelf;
        }

    }
}
