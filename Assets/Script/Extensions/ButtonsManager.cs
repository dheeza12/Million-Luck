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

    [Header("Home Buttons Array")]
    public Button[] homeButton;
    [Header("Obj Buttons Array")]
    public Button[] objButton;

    [Header("Popups")]
    public GameObject battleDesuka;
    public GameObject Home;
    public GameObject objective;

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
