using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnDisplay : MonoBehaviour
{
    private TextMeshProUGUI turnText;
    private void OnEnable()
    {
        GameController.Instance.DiceModeChangeEvent += ChangeText;

    }
    private void OnDisable()
    {
        GameController.Instance.DiceModeChangeEvent -= ChangeText;
    }

    private void Start()
    {
        turnText = GetComponentInChildren<TextMeshProUGUI>();
        ChangeText(GameController.DiceMode.Move);
    }

    private void ChangeText(GameController.DiceMode diceMode)
    {
        turnText.SetText(string.Format("Round {0}, Turn: {1} \n {2}: {3}", 
            GameController.round, GameController.whoseTurn, GameController.currentPlayerAttribute.playerName, diceMode));
    }
}
