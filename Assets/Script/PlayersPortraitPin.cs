using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public  class PlayersPortraitPin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startBattleText;
    public GameObject attacker;
    public GameObject attacked;

    private void OnEnable() {
        SetPortrait();
        if (startBattleText != null)
        {
            startBattleText.SetText(string.Format("Challenge {0} to Quiz battle?", GameController.players_ingame[GameController.gettingAttacked - 1].GetComponent<PlayerAttribute>().playerName));
        }
        
    }
    
    public void SetPortrait()
    {
        SpriteRenderer playerAttacker = GameController.players_ingame[GameController.attacker - 1].GetComponent<SpriteRenderer>();

        attacker.GetComponent<Image>().sprite = playerAttacker.sprite;
        attacker.GetComponent<Image>().color = playerAttacker.color;

        if (GameController.players_ingame[GameController.gettingAttacked - 1].GetComponent<SpriteRenderer>() != null)
        {
            SpriteRenderer playerAttacked = GameController.players_ingame[GameController.gettingAttacked - 1].GetComponent<SpriteRenderer>();
            attacked.GetComponent<Image>().sprite = playerAttacked.sprite;
            attacked.GetComponent<Image>().color = playerAttacked.color;
        }
        
    }
}
