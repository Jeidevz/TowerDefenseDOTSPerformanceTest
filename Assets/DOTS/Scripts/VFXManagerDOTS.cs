using System.Collections;
using System.Collections.Generic;
using Unity.Serialization;
using UnityEngine;

namespace TowerDefenseDOTS
{
    public class VFXManagerDOTS : MonoBehaviour
    {
        [DontSerialize] public static List<GameObject> effects;

        [SerializeField] private VFXLibraryScriptableObject vfxLibrary;
        private static VFXManagerDOTS _instance;

        public static VFXManagerDOTS Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<VFXManagerDOTS>();
                }

                return _instance;
            }
        }
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            effects = new List<GameObject>(vfxLibrary.effects);
        }
    }
}