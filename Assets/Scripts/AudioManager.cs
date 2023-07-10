using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TowerDefense
{
    public class AudioManager : MonoBehaviour
    {
        private static string BGM_GROUP_NAME = "BGM";
        private static string SFX_GROUP_NAME = "SFX";
        private static float LOWEST_DB = -80f;

        [SerializeField] private AudioMixer mixer;
        [SerializeField] private bool muted = false;

        private float beforeMuteVolume;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                if (!muted)
                    Mute();
                else
                    UnMute();
            }
        }

        public float ConvertPercentToDb(float percent)
        {
            return LOWEST_DB + (80f * (percent / 100f));
        }
        public float ConvertDbToPercent(float db)
        {
            return 100f - (100f * db / LOWEST_DB);
        }

        public void SetMasterVolume(float volume)
        {
            mixer.SetFloat("Master", ConvertPercentToDb(volume));
        }

        public void SetBGMVolume(float volume)
        {
            mixer.SetFloat(BGM_GROUP_NAME, ConvertPercentToDb(volume));
        }

        public void SetSFXVolume(float volume)
        {
            mixer.SetFloat(SFX_GROUP_NAME, ConvertPercentToDb(volume));
        }

        public void Mute()
        {
            float db;
            mixer.GetFloat("Master", out db);
            beforeMuteVolume = ConvertDbToPercent(db);
            SetMasterVolume(0);
            muted = true;

            Debug.Log("Mute");
        }

        public void UnMute()
        {
            mixer.SetFloat("Master", ConvertPercentToDb(beforeMuteVolume));
            muted = false;
            Debug.Log("UnMute");
        }


    }
}