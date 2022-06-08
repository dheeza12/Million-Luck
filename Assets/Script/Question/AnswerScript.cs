using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    private ColorBlock buttonColors;
    private ColorBlock defaultColor;
    private void Awake()
    {
        defaultColor = GetComponent<Button>().colors;
    }

    private void OnEnable()
    {
        var color = GetComponent<Button>().colors;
        color.normalColor = defaultColor.normalColor;
        color.selectedColor = defaultColor.selectedColor;
        GetComponent<Button>().colors = color;
    }

    public void Answer() {
        var colors = GetComponent<Button>().colors;
        if (isCorrect)
        {
            colors.normalColor = Color.green;
            colors.selectedColor = Color.green;
        }
        else
        {
            colors.normalColor = Color.red;
            colors.selectedColor = Color.red; 
        }

        GetComponent<Button>().colors = colors;
    }

}
