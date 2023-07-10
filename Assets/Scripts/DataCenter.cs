using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public static class DataCenter
    {
        public delegate void DataCenterUpdatedDelegate();
        public delegate void DataCenterSettingUpdatedDelegate();
        private static float gameScore = 0f;
        public static uint startSpawnAmount = 1000;
        public static uint spawnRate = 20;
        public static float mouseSensitivity = 1f;
        public static float masterVolume = 100f;
        public static float sfxVolume = 100f;
        public static float bgmVolume = 100f;

        private static DataCenterUpdatedDelegate dataCenterUpdatedDelegate;


        public static void AddScore(float score)
        {
            gameScore += score;
            //Check delegate invoke list
            //System.Delegate[] delegates = dataCenterUpdatedDelegate.GetInvocationList();
            //foreach (System.Delegate dele in delegates)
            //{
            //    Debug.Log("Delegate: " + dele.ToString());
            //}

            dataCenterUpdatedDelegate();
        }

        public static void AddDataCenterUpdateDelegate(in DataCenterUpdatedDelegate function)
        {
            //Debug.Log("Function added: " + function.ToString());
            dataCenterUpdatedDelegate += function;
        }

        public static void RemoveDataCenterUpdateDelegate(in DataCenterUpdatedDelegate function)
        {
            //Debug.Log("Function removed: " + function.ToString());
            dataCenterUpdatedDelegate -= function;
        }

        public static float GetGameScore()
        {
            return gameScore;
        }

        public static void ResetDelegates()
        {
            dataCenterUpdatedDelegate = null;
        }

        public static void ResetScore()
        {
            gameScore = 0;
        }
        
    }
}
