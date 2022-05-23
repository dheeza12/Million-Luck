using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AttributeChange;

public class UIComponent : MonoBehaviour
{
    [SerializeField] private PlayerAttribute player;

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

    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI winText;
    
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI constestantNo;
    
    
    private void Start() {
        // Set Name
        if (player.playerName != "")
        {
            playerName.SetText(player.playerName);
        }
        else
        {
            playerName.SetText(player.gameObject.name);
        }
        constestantNo.SetText("CONTESTANT "+ player.name[player.name.Length - 1]);
        
    }

    private void DisableUI(){
        gameObject.SetActive(false);
    }

    private void UpdatePlayerUI(ChangedPoint changedEnum, int hp, int score, int win) {
        hpText.SetText(string.Format("{0}/{1}", hp, player.maxHP));
        scoreText.SetText(score.ToString());
        winText.SetText(win.ToString());
    }
}
