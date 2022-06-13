using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveUIComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI scoreButton;
    [SerializeField] private TextMeshProUGUI winButton;

    private Button scoreButtonComp;
    private Button winButtonComp;

    private void Awake()
    {
        scoreButtonComp = scoreButton.GetComponentInParent<Button>();
        winButtonComp = winButton.GetComponentInParent<Button>();
    }

    private PlayerAttribute player;


    private void OnDisable()
    {
        PlayerAttribute.LevelSelect -= DisplayQuest;
    }

    private void OnEnable()
    {
        PlayerAttribute.LevelSelect += DisplayQuest;
        player = GameController.players_ingame[GameController.whoseTurn - 1].GetComponent<PlayerAttribute>();

        if (!scoreButtonComp.gameObject.activeSelf)
        {
            scoreButtonComp.gameObject.SetActive(true);
        }
        if (!winButtonComp.gameObject.activeSelf)
        {
            winButtonComp.gameObject.SetActive(true);
        }

        objectiveText.SetText("Quest objective not reached");

        switch (player.winCondition)
        {
            case PlayerAttribute.WinCondition.winWin:
                scoreButtonComp.gameObject.SetActive(false);
                winButton.SetText(string.Format("Reach >{0} Win scores", GameController.Instance.winNeed[player.level]));
                break;
            case PlayerAttribute.WinCondition.ScoreWin:
                winButtonComp.gameObject.SetActive(false);
                scoreButton.SetText(string.Format("Reach >{0} Lucky scores", GameController.Instance.scoreNeed[player.level]));
                break;
            default:
                break;
        }
        if (player.level == 0)
        {
            winButton.SetText(string.Format("Have more than {0} Lucky scores", GameController.Instance.winNeed[player.level]));
            scoreButton.SetText(string.Format("Have more than {0} Lucky scores", GameController.Instance.scoreNeed[player.level]));
        }
    }
    
    private void DisplayQuest()
    {
        objectiveText.SetText("Quest objective reached!\nSelect next quest");
        if (!scoreButtonComp.gameObject.activeSelf)
        {
            scoreButtonComp.gameObject.SetActive(true);
        }
        if (!winButtonComp.gameObject.activeSelf)
        {
            winButtonComp.gameObject.SetActive(true);
        }

        if (player.level < GameController.Instance.winNeed.Length)
        {
            winButton.SetText(string.Format("Have more than {0} Win scores", GameController.Instance.winNeed[player.level]));
            scoreButton.SetText(string.Format("Have more than {0} Quiz Money", GameController.Instance.scoreNeed[player.level]));
        }
        
    }

    public void ChangeWinConditionWin()
    {
        player.ChangeWinCondition(PlayerAttribute.WinCondition.winWin);
    }

    public void ChangeWinConditionScore()
    {
        player.ChangeWinCondition(PlayerAttribute.WinCondition.ScoreWin);
    }
}
