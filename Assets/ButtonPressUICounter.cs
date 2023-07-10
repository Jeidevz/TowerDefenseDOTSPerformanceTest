using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonPressUICounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textGUI;
    [SerializeField] string header;
    [SerializeField] KeyCode button;
    [SerializeField] int counter = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(button))
            UpdateCounter();
    }

    void UpdateCounter()
    {
        counter++;
        textGUI.text = header + " " + counter.ToString();
    }

}
