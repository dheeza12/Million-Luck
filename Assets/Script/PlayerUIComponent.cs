using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AttributeChange;

public class PlayerUIComponent : MonoBehaviour
{
    private void OnEnable() 
    {
        player.NotInGame += DisableUI;
        player.StatChanged += UpdatePlayerUI;
    }

    private void OnDisable() 
    {
        if (player != null)
        {
            player.NotInGame -= DisableUI;
            player.StatChanged -= UpdatePlayerUI;    
        }
        
    }
    [SerializeField] private PlayerAttribute player;
    [Header("UI Components")]
    [Header("Hp and win conditions")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI MoneyText;
    [SerializeField] private TextMeshProUGUI winText;
    
    [Header("Identity")]
    [SerializeField] private TextMeshProUGUI constestantNoText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    
    
    private void Start() {
        // Set Name
        if (player.playerName != "")
        {
            playerNameText.SetText(player.playerName);
        }
        else
        {
            playerNameText.SetText(player.gameObject.name);
            player.playerName = player.gameObject.name;
        }
        constestantNoText.SetText("CONTESTANT "+ player.name[player.name.Length - 1]);
        
    }

    private void DisableUI(){
        gameObject.SetActive(false);
    }

    private void UpdatePlayerUI(ChangedPoint changedEnum, int hp, int score, int win) {
        hpText.SetText(string.Format("{0}/{1}", hp, player.maxHP));
        MoneyText.SetText(score.ToString());
        winText.SetText(win.ToString());
    }
}
