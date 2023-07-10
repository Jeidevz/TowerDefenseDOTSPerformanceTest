using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TowerDefense
{
    public class ComboFeedbackUI : MonoBehaviour
    {
        [System.Serializable]
        public struct FeedbackSettingData
        {
            public string feedback;
            public uint comboMin;
            public Color color;
        }

        public struct ComboData
        {
            public uint comboCount;
            public uint acculumatedPoints;
        }

        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private TextMeshProUGUI comboText;
        [SerializeField] private TextMeshProUGUI comboPointsToMultiplyText;
        [SerializeField] private string comboTextHeader;
        [SerializeField] private string multiplyHeader;
        [SerializeField] private Color startColor;
        [Header("Combo text setting")]
        [SerializeField] private uint comboPoint = 100;
        [SerializeField] private uint comboMultiplier = 1;
        [SerializeField] private float comboTextEnlargeEffect = .2f;
        [SerializeField] private float comboTextEnlargeEffectDuration = .1f;
        [Space]
        [SerializeField] private float feedbackDuration = 5f;
        [SerializeField] private float totalComboPointsDuration = 5f;
        [SerializeField] private FeedbackSettingData[] feedbackLvls;

        private Coroutine feedbackTextCoroutine;
        private Coroutine totalComboPointsCoroutine;
        private Coroutine comboEnlargeEffectCoroutine;

        private uint comboCount = 0;
        private int feedbackLvl = 0;
        private float comboTextOriginalYScale;
        private bool showingTotalComboPointsGain = false;
        
        void Start()
        {
            comboTextOriginalYScale = GetComponent<RectTransform>().localScale.y;
            SetFeedbackTextVisibilityState(false);
            SetVisibilityCompoPointsToMultiply(false);
            ResetStyle();
            UpdateComboText(comboCount);
            UpdateComboText(comboCount);
        }

        private void UpdateComboText(uint combo)
        {
            comboText.text = comboTextHeader + combo;
        }

        private void SetVisibilityCompoPointsToMultiply(bool state)
        {
            comboPointsToMultiplyText.enabled = state;
        }

        private void UpdateComboPointsToMultiplyText(float comboPoint, int comboLvl)
        {
            comboPointsToMultiplyText.text = multiplyHeader + (comboPoint * comboLvl);
        }
        private void UpdateComboPointsToMultiplyTextStyle(in FeedbackSettingData data)
        {
            comboPointsToMultiplyText.color = data.color;
        }

        private IEnumerator ShowFeedbackText(float duration)
        {
            float timer = 0;

            while(timer < duration)
            {

                timer += Time.deltaTime;
                yield return null;
            }

            SetFeedbackTextVisibilityState(false);
            yield return null;
        }

        private void ShowFeedbackText()
        {
            if (feedbackTextCoroutine != null)
                StopCoroutine(feedbackTextCoroutine);

            feedbackTextCoroutine = StartCoroutine(ShowFeedbackText(feedbackDuration));
        }

        private void DoComboTextEnlargeEffect(float duration)
        {
            if (comboEnlargeEffectCoroutine != null)
                StopCoroutine(comboEnlargeEffectCoroutine);

            comboEnlargeEffectCoroutine = StartCoroutine(StartComboTextEnlargeEffectCoroutine(comboTextEnlargeEffectDuration));
        }

        private IEnumerator StartComboTextEnlargeEffectCoroutine(float duration)
        {
            RectTransform rectTrans = comboText.GetComponent<RectTransform>();
            float originalYScale = comboTextOriginalYScale;

            while(rectTrans.localScale.y < originalYScale + comboTextEnlargeEffect)
            {
                rectTrans.localScale += Vector3.up * comboTextEnlargeEffect / duration * Time.deltaTime;
                yield return null;
            }

            while(rectTrans.localScale.y > originalYScale)
            {
                rectTrans.localScale -= Vector3.up * comboTextEnlargeEffect / duration * Time.deltaTime;
                yield return null;
            }

            rectTrans.localScale = Vector3.one;
            yield return null;
        }

        private bool IsComboPointsMultiplyTextVisible()
        {
            return comboPointsToMultiplyText.enabled;
        }

        private void CheckScoreForFeedback(uint currentCombo)
        {
            if (ShowedHighestFeedbackLvl())
                return;

            if(currentCombo >= feedbackLvls[feedbackLvl].comboMin)
            {
                FeedbackSettingData feedbackData = feedbackLvls[feedbackLvl];

                UpdateFeedbackTextStyle(in feedbackData);
                UpdateComboTextStyle(in feedbackData);
                UpdateComboPointsToMultiplyTextStyle(in feedbackData);

                SetFeedbackTextVisibilityState(true);

                if (!IsComboPointsMultiplyTextVisible())
                    SetVisibilityCompoPointsToMultiply(true);

                ShowFeedbackText();

                IncreaseFeedbackLvl();
                UpdateComboPointsToMultiplyText(comboPoint, feedbackLvl);
            }
        }

        private void UpdateFeedbackTextStyle(in FeedbackSettingData data)
        {
            feedbackText.color = data.color;
            feedbackText.text = data.feedback;
        }

        private void UpdateComboTextStyle(in FeedbackSettingData data)
        {
            comboText.color = data.color;
        }

        private void SetFeedbackTextVisibilityState(bool state)
        {
            feedbackText.enabled = state;

            if (!state && feedbackTextCoroutine != null)
                StopCoroutine(feedbackTextCoroutine);
        }
        private void ResetStyle()
        {
            comboText.color = startColor;
            feedbackText.color = startColor;
            comboPointsToMultiplyText.color = startColor;
        }

        private bool ShowedHighestFeedbackLvl()
        {
            return (feedbackLvl + 1) > feedbackLvls.Length;
        }

        private void IncreaseFeedbackLvl()
        {
            feedbackLvl++;
        }

        private IEnumerator ShowTotalComboPoints(float duration)
        {
            showingTotalComboPointsGain = true;
            float timer = 0;
            comboPointsToMultiplyText.text = "+" + CalculateAcculumatedComboPoints().ToString();
            while(timer < duration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            showingTotalComboPointsGain = false;

            if(feedbackLvl == 0)
                SetVisibilityCompoPointsToMultiply(false);

            yield return null;
        }

        private uint CalculateAcculumatedComboPoints()
        {
            return comboCount * comboPoint * comboMultiplier * (uint)feedbackLvl;
        }

        private void ShowAcculumatedComboPoints()
        {
            if (totalComboPointsCoroutine != null)
                StopCoroutine(totalComboPointsCoroutine);

            totalComboPointsCoroutine = StartCoroutine(ShowTotalComboPoints(totalComboPointsDuration));
        }

        public void AddCombo()
        {
            comboCount++;
            UpdateComboText(comboCount);
            DoComboTextEnlargeEffect(comboTextEnlargeEffectDuration);
            CheckScoreForFeedback(comboCount);
        }

        public void ComboBroken(out ComboData data)
        {
            data.comboCount = comboCount;
            data.acculumatedPoints = CalculateAcculumatedComboPoints();

            ComboBroken();
        }

        public void ComboBroken()
        {
            if(feedbackLvl > 0)
                ShowAcculumatedComboPoints();

            UpdateFeedbackTextStyle(new FeedbackSettingData { color = Color.black, feedback = "Combo broken!" });
            ShowFeedbackText();
            ShowFeedbackText(feedbackDuration);

            ResetCombo();
            ResetStyle();
            UpdateComboText(0);
        }

        public void ResetCombo()
        {
            comboCount = 0;
            feedbackLvl = 0;
        }

    }
}
