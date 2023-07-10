using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private bool showCursor = true;
        [SerializeField] private CursorLockMode lockCursorState = CursorLockMode.Locked;
        [Header("Scoreboard")]
        [SerializeField] private Scoreboard scoreboard;
        [SerializeField] ComboFeedbackUI comboFeedbackUI;

        [Header("Missile")]
        [SerializeField] private MissileHolder missileHolder;
        [SerializeField] private ChargeMeter chargeMeter;
        [SerializeField] private float chargeAmountPerKill = 0.5f;

        private EnemyCapsule.OnDeath enemyOnDeathListener;

        private void Awake()
        {
            EnemyCapsule.ResetDelegates();
            DataCenter.ResetDelegates();
            DataCenter.ResetScore();
            //UnPauseGame();
        }

        void Start()
        {
            ChangeCursorSetting(showCursor, lockCursorState);
            enemyOnDeathListener = OnEnemyDeath;
            EnemyCapsule.RemoveDeathListener(enemyOnDeathListener);
            EnemyCapsule.AddDeathListener(enemyOnDeathListener);
        }

        //For testing purpose
        private void Update()
        {
            //Manual combo break testing.
            if(Input.GetKeyDown(KeyCode.B))
            {
                ComboFeedbackUI.ComboData comboData;
                comboFeedbackUI.ComboBroken(out comboData);
                AddScore((float)comboData.acculumatedPoints);
            }
        }

        private void OnEnemyDeath()
        {
            comboFeedbackUI.AddCombo();

            if (missileHolder.IsFull())
                return;
            else
                chargeMeter.increase(chargeAmountPerKill);

            if(chargeMeter.IsMaxed())
            {
                if (missileHolder.LoadMissile())
                    chargeMeter.Reset();
            }
        }

        public void ChangeCursorSetting(bool showState, CursorLockMode lockState)
        {
            Cursor.visible = showState;
            Cursor.lockState = lockState;
        }

        public void GoToMainScreen()
        {
            SceneManager.LoadScene("StartScene");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public static void AddScore(float score)
        {
            //sScoreboard.AddScore(score);
            DataCenter.AddScore(score);
        }

        static public void PauseGame()
        {
            Time.timeScale = 0;
        }

        static public void UnPauseGame()
        {
            Time.timeScale = 1;
        }


    }
}
