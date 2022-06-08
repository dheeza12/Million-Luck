using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AttributeChange;
using UnityEngine.UI;

public class PlayerUIComponent : MonoBehaviour
{
    [SerializeField] private PlayerAttribute player;
    [Header("UI Components")]
    [Header("Hp, score, and win")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI MoneyText;
    [SerializeField] private TextMeshProUGUI winText;

    [Header("Identity")]
    [SerializeField] private TextMeshProUGUI constestantNoText;
    [SerializeField] private TextMeshProUGUI playerNameText;

    private Sprite[] questSprite;
    [Header("Level text, sprite")]

    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI neededLevel;
    [SerializeField] private Image displayQuest;



    private void OnEnable() 
    {
        player.NotInGame += DisableUI;
        player.StatChanged += UpdatePlayerUI;
        PlayerAttribute.LevelSelect += UpdateLevel;
    }

    private void OnDisable() 
    {
        if (player != null)
        {
            player.NotInGame -= DisableUI;
            player.StatChanged -= UpdatePlayerUI;
            PlayerAttribute.LevelSelect -= UpdateLevel;
        }
        
    }
    
    
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

        questSprite = Resources.LoadAll<Sprite>("Currency/");

    }

    private void DisableUI(){
        gameObject.SetActive(false);
    }

    private void UpdatePlayerUI(ChangedPoint changedEnum, int hp, int score, int win) {
        hpText.SetText(string.Format("{0}/{1}", player.hp, player.maxHP));
        MoneyText.SetText(score.ToString());
        winText.SetText(win.ToString());
    }

    private void UpdateLevel()
    {
        level.SetText(string.Format("Level: {0}", player.level+1));
        if (player.level < GameController.Instance.winNeed.Length)
        {
            switch (player.winCondition)
            {
                case PlayerAttribute.WinCondition.winWin:
                    neededLevel.SetText(string.Format("Needed {0}", GameController.Instance.winNeed[player.level]));
                    displayQuest.sprite = questSprite[0];
                    break;
                case PlayerAttribute.WinCondition.ScoreWin:
                    neededLevel.SetText(string.Format("Needed {0}", GameController.Instance.scoreNeed[player.level]));
                    displayQuest.sprite = questSprite[1];
                    break;
                default:
                    break;
            }
        }
        
    }
}
