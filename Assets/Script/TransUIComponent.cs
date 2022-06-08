using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TransUIComponent : MonoBehaviour
{
    // DiceMode{Move, Attack, Defend, FreePoint, LosePoint, QuizMode}

    private void OnEnable()
    {
        GameController.Instance.DiceModeChangeEvent += ChangeMode;
    }
    private void OnDisable() {
        GameController.Instance.DiceModeChangeEvent -= ChangeMode;
    }

    private int curPlayer;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private CanvasGroup background;
    private CanvasGroup selfAlpha;

    private void Start()
    {
        selfAlpha = GetComponent<CanvasGroup>();
        TransitionTurn();
    }

    private void TransitionTurn()
    {
        SetVisible();
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);
        
        textBox.transform.localPosition = new Vector2(Screen.width * 2, 0);
        textBox.transform.LeanMoveLocalX(0, 0.66f).setEaseInOutExpo().setOnComplete(SetInvisible).delay = 0.1f;
        
    }

    private void TransitionMode()
    {
        // reset state
        SetVisible();
        // set if whole overlay bg is to cover whole screen
        background.alpha = 0;

        textBox.transform.localPosition = new Vector2(0, -Screen.height);
        textBox.transform.LeanMoveLocalY(0, 0.66f).setEaseInOutExpo().setOnComplete(SetInvisible).delay = 0.1f;
        
    }


    private void SetVisible()
    {
        selfAlpha.alpha = 0;
        selfAlpha.LeanAlpha(1, 0.5f);
        selfAlpha.interactable = true;
        selfAlpha.blocksRaycasts = true;
    }
    private void SetInvisible()
    {
        selfAlpha.LeanAlpha(0, 0.66f).delay = delayTime;
        selfAlpha.interactable = false;
        selfAlpha.blocksRaycasts = false;
    }

    private float delayTime = 0.66f;
    private void ChangeMode(GameController.DiceMode newDiceMode){
        LeanTween.cancel(selfAlpha.gameObject);
        LeanTween.cancel(background.gameObject);
        LeanTween.cancel(textBox.gameObject);
        switch (newDiceMode)
        {
            // Move is only when NewTurn() is run'd
            case GameController.DiceMode.Move:
                textBox.SetText(string.Format("Constestant {0}'s Turn", GameController.whoseTurn));
                TransitionTurn();
                break;
            case GameController.DiceMode.DoubleMove:
                textBox.SetText("Move 1 more time");
                TransitionMode();
                break;
            case GameController.DiceMode.FreePoint:
                textBox.SetText("Free Score");
                TransitionMode();
                break;
            case GameController.DiceMode.LosePoint:
                textBox.SetText("Dropping score");
                TransitionMode();
                break;
            case GameController.DiceMode.Revive:
                textBox.SetText(string.Format("Constestant {0}'s Roll to revive", GameController.whoseTurn));
                TransitionMode();
                break;
            default:
                break;
        }

    }
}
