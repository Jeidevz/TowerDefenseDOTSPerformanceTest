using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class AnimatorSetRanromIntValue : MonoBehaviour
    {
        [SerializeField] private string parameterName;
        [SerializeField] private int min;
        [SerializeField] private int max;
        // Start is called before the first frame update
        void Start()
        {
            Animator anim = GetComponent<Animator>();
            anim.SetInteger(parameterName, Random.Range(min, max + 1));
        }
    }
}
