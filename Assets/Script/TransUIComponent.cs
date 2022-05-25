using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TransUIComponent : MonoBehaviour
{
    // DiceMode{Move, Attack, Defend, FreePoint, LosePoint, QuizMode}
    private void OnEnable() {
        GameController.DiceModeChangeDel += ChangeMode;
    }

    private void OnDisable() {
        GameController.DiceModeChangeDel -= ChangeMode;
    }

    private int curPlayer;
    [SerializeField] private TextMeshProUGUI curDisplay;

    private void ChangeMode(GameController.DiceMode newDiceMode){
        curPlayer = GameController.whoseTurn;
        string display = string.Format("Player: {0}\n Mode: {1}", curPlayer, newDiceMode);
        curDisplay.SetText(display);
    }
}
