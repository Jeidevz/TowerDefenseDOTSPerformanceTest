using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIChildCounter : MonoBehaviour
{
    public TextMeshProUGUI tmProGui;
    public Transform parent;
    public string header;
    //TODO: This cause alot of work for GC. Make it change upon enemy death or spawn. Static would be liekly.
    void FixedUpdate()
    {
        tmProGui.SetText(header + parent.childCount);
    }
}
