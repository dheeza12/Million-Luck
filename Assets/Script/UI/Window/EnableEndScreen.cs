using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnableEndScreen : MonoBehaviour
{
    [SerializeField] private GameObject endWindow;
    [SerializeField] private TextMeshProUGUI contestantDetail, gamePlayed, winnerStat;

    private void OnEnable()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().LeanAlpha(1, 0.66f);

        endWindow.transform.localPosition = new Vector2(0, Screen.height*2);
        endWindow.LeanMoveLocalY(0f, 2f).setEaseOutQuint().delay = 0.66f;

        contestantDetail.SetText(string.Format("Contestant{0}, \n{1}", GameController.whoseTurn, GameController.currentPlayerAttribute.playerName));
        gamePlayed.SetText(string.Format("Game\nround played: {0}", GameController.round));
        string endMode = "won by number of Wins";
        if (GameController.currentPlayerAttribute.winCondition == PlayerAttribute.WinCondition.ScoreWin)
        {
            endMode = "won by number of Scores";
        }
        winnerStat.SetText(string.Format("Winner\nlast score point: {0}\nlast wins: {1}\nwinning quest: {2}",
            new object[] {
            GameController.currentPlayerAttribute.score,
            GameController.currentPlayerAttribute.win,
            endMode
            }
            ));
    }
}
