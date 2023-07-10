using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class SettingUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SettingManager settingManager;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private RectTransform windowRect;
        [SerializeField] private float hideWidth = 450f;
        [Header("Inputs")]
        [SerializeField] private TMP_InputField startAmountInputField;
        [SerializeField] private TMP_InputField spawnRateInputField;
        [SerializeField] private UISliderValueVizualizer mouseSensSliderVisualizer;
        [SerializeField] private UISliderValueVizualizer masterVolSliderVisualizer;
        [SerializeField] private UISliderValueVizualizer SFXVolSliderVisualizer;
        [SerializeField] private UISliderValueVizualizer BGMVolSliderVisualizer;

        [SerializeField] bool opened = true;

        // Start is called before the first frame update
        void Start()
        {
            if (!opened)
                Hide();
            else
                Show();
        }

        //// Update is called once per frame
        //void Update()
        //{
        //    //Test
        //    if (Input.GetKeyDown(InputManager.option))
        //    {
        //        if (opened)
        //            Hide();
        //        else
        //            Show();
        //    }
        //}

        private void Hide()
        {
            windowRect.anchoredPosition = new Vector2(windowRect.rect.width, windowRect.anchoredPosition.y);
            opened = false;
        }

        private void Show()
        {
            windowRect.anchoredPosition = Vector3.zero;
            opened = true;
        }


        private void ChangeStartAmountText(uint amount)
        {
            startAmountInputField.text = amount.ToString();
        }

        private void ChangeSpawnRateText(uint amount)
        {
            spawnRateInputField.text = amount.ToString();
        }

        private void ChangeMouseSensitivitySliderValue(float value)
        {
            mouseSensSliderVisualizer.ChangeSliderValue(value);
        }

        private void ChangeMasterVolumeSliderValue(float value)
        {
            masterVolSliderVisualizer.ChangeSliderValue(value);
        }

        private void ChangeSFXVolumeSliderValue(float value)
        {
            SFXVolSliderVisualizer.ChangeSliderValue(value);
        }

        private void LoadCurrentValuesToUIs()
        {
            settingManager.LoadDataCenterValuesToSettingUI();
        }

        public void ChangeBGMVolumeSliderValue(float value)
        {
            BGMVolSliderVisualizer.ChangeSliderValue(value);
        }

        public void ApplySetting()
        {
            settingManager.ChangeSpawnsAtStart(uint.Parse(startAmountInputField.text));
            settingManager.ChangeSpawnRate(uint.Parse(spawnRateInputField.text));
            settingManager.ChangeMouseSensitivity(mouseSensSliderVisualizer.GetSliderValue());
            settingManager.ChangeMasterVolume(masterVolSliderVisualizer.GetSliderValue());
            settingManager.ChangeSFXVolume(SFXVolSliderVisualizer.GetSliderValue());
            settingManager.ChangeBGMVolume(BGMVolSliderVisualizer.GetSliderValue());
        }

        public void CancelSetting()
        {
            LoadCurrentValuesToUIs();
            Hide();
            gameManager.ChangeCursorSetting(false, CursorLockMode.Locked);
        }

        //TODO: Not sure how it will handle but find out!
        public void Restart()
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }

        public void ChangeSpawnRate(uint spawnRate)
        {
            ChangeSpawnRateText(spawnRate);
        }

        public void ChangeSpawnsAtStart(uint amount)
        {
            ChangeStartAmountText(amount);
        }

        public void ChangeMouseSensitivity(float value)
        {
            ChangeMouseSensitivitySliderValue(value);
        }

        public void ChangeMasterVolume(float value)
        {
            ChangeMasterVolumeSliderValue(value);
        }

        public void ChangeSFXVolume(float value)
        {
            ChangeSFXVolumeSliderValue(value);
        }

        public void ChangeBGMVolume(float value)
        {
            ChangeBGMVolumeSliderValue(value);
        }

        public bool IsOpen()
        {
            return opened;
        }

        public void Open()
        {
            Show();
        }

        public void Close()
        {
            Hide();
        }
    }
}