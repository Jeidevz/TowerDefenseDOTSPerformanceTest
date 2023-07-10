using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Slot : MonoBehaviour
    {
        public void PlaceToSlot(Transform item)
        {
            item.parent = transform;
            item.localPosition = Vector3.zero;
        }

        public GameObject GetSlotItem()
        {
            return transform.GetChild(0).gameObject;
        }

        public bool IsEmpty()
        {
            return transform.childCount == 0;
        }
    }
}
