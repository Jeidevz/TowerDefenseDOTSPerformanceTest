using System.Collections;
using System.Collections.Generic;
using Unity.Serialization;
using UnityEngine;

namespace TowerDefenseDOTS
{
    public class SoundManager : MonoBehaviour
    {
        [DontSerialize] public static List<AudioClip> SfxClips;

        [SerializeField] private SoundLibraryScriptableObject sfxSoundLibrary;
        private static SoundManager _instance;

        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<SoundManager>();
                }

                return _instance;
            }
        }

        

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SfxClips = new List<AudioClip>(sfxSoundLibrary.clips);
        }


    }
}