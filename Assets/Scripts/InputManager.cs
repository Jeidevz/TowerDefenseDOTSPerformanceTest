using System.Collections;
using System.Runtime.CompilerServices;
using TowerDefenseSim;
using UnityEngine;

namespace TowerDefense
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] GameManager gameManager;
        [SerializeField] SettingManager settingManager;
        [SerializeField] ThirdPersonCamera thirdPersonCam;
        [SerializeField] private SettingUI settingUI;
        [SerializeField] private MainMenuUI mainMenuUI;
        [SerializeField] private Turrent turrent;

        static public KeyCode fireTurrents = KeyCode.Mouse0;
        static public KeyCode fireMissile = KeyCode.Mouse1;
        static public KeyCode reloadMissile = KeyCode.R;
        static public KeyCode option = KeyCode.O;
        static public KeyCode moveLeft = KeyCode.A;
        static public KeyCode moveright = KeyCode.D;
        static public KeyCode openMainMenu = KeyCode.Escape;
        private void Update()
        {
            if (!IsAnyUIOpen())
                PlayerControls();

            UIControls();
        }

        private void PlayerControls()
        {

            TurrentControls();
            MovementControls();
            
        }

        private void UIControls()
        {
            if (Input.GetKeyDown(InputManager.option))
            {
                if (!settingUI.IsOpen())
                    OpenSettingWindow();
                else
                    CloseSettingWindow();
            }
            else if(Input.GetKeyDown(InputManager.openMainMenu))
            {
                if (!settingUI.IsOpen())
                    OpenMainMenu();
                else
                    CloseMainMenu();
            }
        }

        private void FireTurrentGuns()
        {
            turrent.FireTurrentGuns();
        }

        private void FireMissile()
        {
            turrent.FireMissile();
        }

        private bool ReloadMissile()
        {
            return turrent.ReloadMissile();
        }

        private void TurrentControls()
        {
            if (Input.GetKey(InputManager.fireTurrents))
                FireTurrentGuns();

            if (Input.GetKeyDown(InputManager.fireMissile))
                FireMissile();
            else if (Input.GetKeyDown(InputManager.reloadMissile))
                ReloadMissile();
        }

        private void OpenMainMenu()
        {

            if (!IsAnyUIOpen())
            {
                SetCameraActiveState(false);
                gameManager.ChangeCursorSetting(true, CursorLockMode.None);
            }

            mainMenuUI.Open();
        }

        private void CloseMainMenu()
        {
            mainMenuUI.Close();


            if (IsAnyUIOpen())
                return;

            SetCameraActiveState(true);
            gameManager.ChangeCursorSetting(false, CursorLockMode.Locked);
        }

        private void OpenSettingWindow()
        {

            if (!IsAnyUIOpen())
            {
                SetCameraActiveState(false);
                gameManager.ChangeCursorSetting(true, CursorLockMode.None);
            }

            settingUI.Open();
        }

        private bool IsAnyUIOpen()
        {
            return (mainMenuUI.IsOpen() || settingUI.IsOpen());
        }

        private void CloseSettingWindow()
        {
            settingUI.Close();
            SetCameraActiveState(true);
            gameManager.ChangeCursorSetting(false, CursorLockMode.Locked);
            settingManager.LoadDataCenterValuesToSettingUI();
        }

        private void MovementControls()
        {
            if (Input.GetKey(moveLeft))
                MoveLeft();
            else if (Input.GetKey(moveright))
                MoveRight();
        }

        private void MoveLeft()
        {

        }

        private void MoveRight()
        {

        }

        private void SetCameraActiveState(bool state)
        {
            thirdPersonCam.SetRotatable(state);
        }
    }
}