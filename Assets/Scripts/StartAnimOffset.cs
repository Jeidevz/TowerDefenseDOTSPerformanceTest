using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class StartAnimOffset : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

            Animator anim = GetComponent<Animator>();
            AnimatorClipInfo[] animClipInfo = anim.GetCurrentAnimatorClipInfo(0);

            float randomIdleStart = Random.Range(0, anim.GetCurrentAnimatorStateInfo(0).length);
            anim.Play(animClipInfo[0].clip.name, 0, randomIdleStart);
            //Debug.Log("Clip length: " + anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
