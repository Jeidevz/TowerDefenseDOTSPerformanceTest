using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class MissileHolder : MonoBehaviour
    {
        private Coroutine organizeCoroutine;
        public struct MissileHolderData
        {
            public int missileCount;
            public uint currentCapacity;
        }

        public delegate void MissileAmountChanged(in MissileHolderData data);
    

        [SerializeField] private GameObject prefab;
        [SerializeField] private float detachForce = 10f;
        [SerializeField] private float upwardModifier = 1f;
        [Header("Organize setting")]
        //[SerializeField] private uint capacity = 4;
        [SerializeField] private float space = 0.5f;
        [SerializeField] private bool loadedAtStart = true;
        [SerializeField] private Slot[] slots;


        //private static MissileAmountChanged missileAmountChangedDelegation;
        private MissileAmountChanged missileAmountChangedDelegation;

        private void Start()
        {
            if(loadedAtStart)
            {
                for(int i = 0; i < slots.Length; i++)
                {
                    LoadMissile();
                }
            }

            InvokeMissileAmountChanged();
        }

        private bool IsLoadable()
        {
            return MissileCount() < slots.Length;
        }

        private void ReOrganizeMissilesPosition(float delay)
        {
            if (MissileCount() <= 0)
                return;

            if(organizeCoroutine != null)
                StopCoroutine(organizeCoroutine);

            organizeCoroutine = StartCoroutine("OrganizingMissile", delay);

            //if (MissileCount() > 1)
            //{
            //    //float offset = (transform.childCount - 1) * space / 2;
            //    float offset = capacity * space / 2;
            //    foreach (Transform child in transform)
            //    {
            //        child.position = transform.position + transform.right * offset;
            //        offset -= space;
            //    }
            //}
            //else
            //    transform.GetChild(0).transform.position = transform.position;
        }

        private void InvokeMissileAmountChanged()
        {
            MissileHolderData data;
            data.missileCount = MissileCount();
            data.currentCapacity = (uint)slots.Length;
            missileAmountChangedDelegation?.Invoke(in data);
        }

        private IEnumerator OrganizingMissile(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (MissileCount() > 1)
            {
                float offset = slots.Length * space / 2;
                foreach (Transform child in transform)
                {
                    child.position = transform.position + transform.right * offset;
                    offset -= space;
                }
            }
        }

        private void LoadMissileToEmptySlot(Transform item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsEmpty())
                {
                    slots[i].PlaceToSlot(item);
                    break;
                }
            }
        }

        private bool GetFirstSlotThatHaveItem(out Slot outSlot)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (!slots[i].IsEmpty())
                {
                    outSlot = slots[i];
                    return true;
                }
            }

            outSlot = null;
            return false;
        }

        private bool GetFirstAvailableMissile(out Missile outMissile)
        {
            Slot slot;
            if(GetFirstSlotThatHaveItem(out slot))
            {
                outMissile = slot.GetSlotItem().GetComponent<Missile>();
                return true;
            }

            outMissile = null;
            return false;
        }

        public int MissileCount()
        {
            int count = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (!slots[i].IsEmpty())
                    count++;
            }

            return count;
        }

        public bool isMissileAvailable()
        {
            return MissileCount() > 0;
        }

        public bool IsFull()
        {
            return MissileCount() == slots.Length;
        }

        public bool LaunchMissile(in Vector3 targetPos)
        {
            if (isMissileAvailable())
            {
                //Transform child = transform.GetChild(0);
                //Missile missile = child.GetComponent<Missile>();
                Missile missile;
                if (GetFirstAvailableMissile(out missile))
                {
                    missile.LaunchMissile(targetPos, (-transform.forward + Vector3.up * upwardModifier).normalized, detachForce);
                    //ReOrganizeMissilesPosition(1f);
                    InvokeMissileAmountChanged();
                    return true;
                }
            }

            return false;
        }

        public bool LoadMissile()
        {
            if(IsLoadable())
            {
                GameObject newMissile = Instantiate(prefab, transform.position, Quaternion.LookRotation(Vector3.up), transform);
                //ReOrganizeMissilesPosition(0);
                LoadMissileToEmptySlot(newMissile.transform);
                InvokeMissileAmountChanged();
                return true;
            }

            return false;
        }

        //public static void AddMissileChangeListener(in MissileAmountChanged newListener)
        //{
        //    missileAmountChangedDelegation += newListener;
        //}
        //public static void RemoveMissileChangeListener(in MissileAmountChanged oldListener)
        //{
        //    missileAmountChangedDelegation -= oldListener;
        //}

        public void AddMissileChangeListener(in MissileAmountChanged newListener)
        {
            missileAmountChangedDelegation += newListener;
        }
        public void RemoveMissileChangeListener(in MissileAmountChanged oldListener)
        {
            missileAmountChangedDelegation -= oldListener;
        }
    }
}
