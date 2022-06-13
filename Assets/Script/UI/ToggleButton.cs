using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ToggleButton : MonoBehaviour
{
    public bool toggled = false;
    private ColorBlock buttonColors;
    private ColorBlock defaultColor;
    private TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        buttonColors = GetComponent<Button>().colors;
        defaultColor = buttonColors;
    }

    public void ToggleThisButton()
    {
        toggled = !toggled;
        var colors = GetComponent<Button>().colors;
        if (toggled)
        {
            text.SetText("Unjoin");
            colors.selectedColor = defaultColor.selectedColor;
            colors.normalColor = defaultColor.selectedColor;
        }
        else
        {
            text.SetText("Join");
            colors.selectedColor = defaultColor.normalColor;
            colors.normalColor = defaultColor.normalColor;
        }
        GetComponent<Button>().colors = colors;
    }
}
