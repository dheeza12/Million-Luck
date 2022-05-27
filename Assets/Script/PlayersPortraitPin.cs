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
        SpriteRenderer playerAttacker = GameController.players_ingame[GameController.attacker - 1].GetComponent<SpriteRenderer>();
        SpriteRenderer playerAttacked = GameController.players_ingame[GameController.gettingAttacked - 1].GetComponent<SpriteRenderer>();
        startBattleText.SetText(string.Format("Challenge {0} to Quiz battle?", GameController.players_ingame[GameController.gettingAttacked -1].GetComponent<PlayerAttribute>().playerName));
        attacker.transform.GetChild(0).GetComponent<Image>().sprite = playerAttacker.sprite;
        attacker.transform.GetChild(0).GetComponent<Image>().color = playerAttacker.color;
        attacked.transform.GetChild(0).GetComponent<Image>().sprite = playerAttacked.sprite;
        attacked.transform.GetChild(0).GetComponent<Image>().color = playerAttacked.color;
    }
       
}
