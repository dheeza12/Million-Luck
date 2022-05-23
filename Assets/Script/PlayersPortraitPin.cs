using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public  class PlayersPortraitPin : MonoBehaviour
{
    public GameObject attacker;
    public GameObject attacked;

    private void OnEnable() {
        Sprite playerAttacker = GameController.players_ingame[GameController.attacker - 1].GetComponent<SpriteRenderer>().sprite;
        Sprite playerAttacked = GameController.players_ingame[GameController.gettingAttacked - 1].GetComponent<SpriteRenderer>().sprite;
        attacker.transform.GetChild(0).GetComponent<Image>().sprite = playerAttacker;
        attacked.transform.GetChild(0).GetComponent<Image>().sprite = playerAttacked;
    }
       
}
