using performanceproject;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using TowerDefenseSim;
using UnityEngine;

namespace TowerDefense
{ 
    public class SettingManager : MonoBehaviour
    {
        [Header("Controls")]
        [SerializeField] private float mouseSensitivity = 20f;
        [SerializeField] private SettingUI settingUI;
        [SerializeField] private SpawnManager smAtStart;
        [SerializeField] private SpawnManager smOverTime;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private ThirdPersonCamera thirdPersonCamera;

        private void Awake()
        {
            LoadSettingValuesFromDataCenter();
        }

        private void LoadSettingValuesFromDataCenter()
        {
            ChangeSpawnsAtStart(DataCenter.startSpawnAmount);
            ChangeSpawnRate(DataCenter.spawnRate);
            ChangeMouseSensitivity(DataCenter.mouseSensitivity);
            ChangeMasterVolume(DataCenter.masterVolume);
            ChangeSFXVolume(DataCenter.sfxVolume);
            ChangeBGMVolume(DataCenter.bgmVolume);
            LoadDataCenterValuesToSettingUI();
        }

        private void UpdateSpawnRateData(uint spawnRate)
        {
            DataCenter.spawnRate = spawnRate;
        }

        private void UpdateSpawnsAtStartData(uint amount)
        {
            DataCenter.startSpawnAmount = amount;
        }

        private void UpdateMouseSensitivityData(float value)
        {
            DataCenter.mouseSensitivity = value;
        }

        private void UpdateMasterVolumeData(float value)
        {
            DataCenter.masterVolume = value;
        }

        private void UpdateSFXVolumeData(float value)
        {
            DataCenter.sfxVolume = value;
        }

        private void UpdateBGMVolumeData(float value)
        {
            DataCenter.bgmVolume = value;
        }

        public void LoadDataCenterValuesToSettingUI()
        {
            settingUI.ChangeSpawnsAtStart(DataCenter.startSpawnAmount);
            settingUI.ChangeSpawnRate(DataCenter.spawnRate);
            settingUI.ChangeMasterVolume(DataCenter.masterVolume);
            settingUI.ChangeSFXVolume(DataCenter.sfxVolume);
            settingUI.ChangeBGMVolume(DataCenter.bgmVolume);
            settingUI.ChangeMouseSensitivity(DataCenter.mouseSensitivity);
        }

        public void ChangeSpawnRate(uint spawnRate)
        {
            smOverTime.SetInterval(1f / spawnRate);
            UpdateSpawnRateData(spawnRate);
        }

        public void ChangeSpawnsAtStart(uint amount)
        {
            smAtStart.SetAmount(amount);
            UpdateSpawnsAtStartData(amount);
        }

        //TODO: Set the rest settings
        public void ChangeMouseSensitivity(float value)
        {
            thirdPersonCamera.SetRotateSensitivity(value);
            UpdateMouseSensitivityData(value);
        }

        public void ChangeMasterVolume(float value)
        {
            audioManager.SetMasterVolume(value);
            UpdateMasterVolumeData(value);
        }

        public void ChangeSFXVolume(float value)
        {
            audioManager.SetSFXVolume(value);
            UpdateSFXVolumeData(value);
        }

        public void ChangeBGMVolume(float value)
        {
            audioManager.SetBGMVolume(value);
            UpdateBGMVolumeData(value);
        }

    }
}
