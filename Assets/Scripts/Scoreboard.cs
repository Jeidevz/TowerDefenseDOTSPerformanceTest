using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TowerDefense
{
    public class Scoreboard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textPro;
        [SerializeField] private float incrementSpeed = 0;
        [SerializeField] private string header;

        private float dynamicScore = 0;
        private float totalScore = 0;
        private DataCenter.DataCenterUpdatedDelegate dataCenterUpdateListener;

        void Start()
        {

            dataCenterUpdateListener = OnDataCenterUpdate;
            DataCenter.RemoveDataCenterUpdateDelegate(dataCenterUpdateListener);
            DataCenter.AddDataCenterUpdateDelegate(dataCenterUpdateListener);
        }

        //TODO: Produce alot garbage for GC
        void FixedUpdate()
        {
            if (dynamicScore == totalScore)
                return;

            IncrementDynamicScore();

            int wholeValue = (int)dynamicScore;

            UpdateNewScoreText(wholeValue);
        }

        private void UpdateNewScoreText(int newScore)
        {
            textPro.SetText(header + newScore);
        }

        private void IncrementDynamicScore()
        {

            if (dynamicScore < totalScore)
            {
                if (incrementSpeed > 0)
                {
                    //Speed up the score incrementing
                    float multiplier = (totalScore - dynamicScore) / incrementSpeed;
                    if (multiplier < 1)
                        multiplier = 1;

                    dynamicScore += incrementSpeed * multiplier * Time.deltaTime;
                }
            }
            else
                dynamicScore = totalScore;
        }

        public void AddScore(float score)
        {
            totalScore += score;
        }

        public void UpdateTotalScore(float newScore)
        {
            totalScore = newScore;
        }

        public void OnDataCenterUpdate()
        {
            totalScore = DataCenter.GetGameScore();
        }
    }
}
