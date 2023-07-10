using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TowerDefense
{
    public class MissileUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textUI;
        [SerializeField] private string header;
        [SerializeField] private MissileHolder missileHolder;

        private MissileHolder.MissileAmountChanged missileAmountListener;

        // Start is called before the first frame update
        void Awake()
        {
            missileAmountListener = OnMissileAmountChanged;
            missileHolder.RemoveMissileChangeListener(missileAmountListener);
            missileHolder.AddMissileChangeListener(missileAmountListener);
        }

        private void OnMissileAmountChanged(in MissileHolder.MissileHolderData data)
        {
            textUI.SetText(header + "[" + data.missileCount + "/" + data.currentCapacity + "]");
        }
    }
}
