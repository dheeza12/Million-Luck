using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayersSetting : MonoBehaviour
{
    public static PlayersSetting instance;
    public Image[] playersSprite;
    public TMP_InputField[] playersName;
    public bool[] inGame;
    [SerializeField] private ToggleButton[] toggleButton;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }

    public void StartTheGame()
    {
        for (int i = 0; i < toggleButton.Length; i++)
        {
            inGame[i] = toggleButton[i].toggled;
        }

        if (countPlayersJoining >= 2)
        {
            SceneManager.LoadScene(1);
        }
        else // pop up warning
        {

        }
    }

    public int countPlayersJoining 
    { get 
        { 
            int member = 0; 
            foreach (bool playerPlay in inGame)
            {
                if (playerPlay)
                {
                    member++;
                }
            }
            return member;
        } 
    }

    private int CountPlayer()
    {
        int member = 0;
        foreach (bool playerPlay in inGame)
        {
            if (playerPlay)
            {
                member++;
            }
        }
        return member;
    }

}
