using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    [Header("Waypoint Indicator Buttons Array")]
    public Button[] buttonsWaypoint;

    [Header("Battle Buttons Array")]
    public Button[] buttonsBattle;

    [Header("Battle Prompt UI")]
    public GameObject battleDesuka;

    public static ButtonsManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
