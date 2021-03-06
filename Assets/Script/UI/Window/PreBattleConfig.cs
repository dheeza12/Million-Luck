using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AttributeChange;

public class PreBattleConfig : MonoBehaviour
{
    [Header("UI settings")]
    [SerializeField] private TextMeshProUGUI[] names;

    [SerializeField] private TextMeshProUGUI[] hpDamage;

    [SerializeField] private TextMeshProUGUI turnAnnotate;

    [Header("Dice Transform")]
    [SerializeField] private Transform diceTransform;
    [Header("Dice Position")]
    [SerializeField] private RectTransform[] dicePos;

    PlayerAttribute[] players = new PlayerAttribute[2];

    private void OnEnable()
    {
        PlayerAttribute.BattleStatChanged += StartUpdateDisplay;
        turnAnnotate.SetText("Attacking");
        turnAnnotate.alignment = TextAlignmentOptions.Left;
        // Getting players in a turn
        players[0] = GameController.players_ingame[GameController.attacker - 1].GetComponent<PlayerAttribute>();
        players[1] = GameController.players_ingame[GameController.gettingAttacked - 1].GetComponent<PlayerAttribute>();
        foreach (TextMeshProUGUI text in hpDamage)
        {
            text.SetText("");
        }
        StartCoroutine(Start());
        SetPlayerDisplay();
    }

    private void OnDisable()
    {
        PlayerAttribute.BattleStatChanged -= StartUpdateDisplay;
    }

    IEnumerator Start()
    {
        // wait for 1 frame to let UI be set,
        yield return new WaitForEndOfFrame();

        if (GameController.whoseTurn == GameController.attacker)
        {
            diceTransform.position = dicePos[0].position;
        }
        else
        {
            diceTransform.position = dicePos[1].position;
        }

    }

    private void SetPlayerDisplay()
    {
        for (int i = 0; i < names.Length; i++)
        {
            names[i].SetText(string.Format("{0}", players[i].playerName));
            dicePos[i].gameObject.GetComponent<TextMeshProUGUI>().SetText("");
        }

    }

    private void StartUpdateDisplay(ChangedPoint mode)
    {
        StartCoroutine(UpdateDisplay(mode));
    }

    private IEnumerator UpdateDisplay(ChangedPoint mode)
    {
        yield return new WaitForSeconds(0.22f);
        if (GameController.whoseTurn == GameController.attacker)
        {
            diceTransform.position = dicePos[0].position;
            turnAnnotate.alignment = TextAlignmentOptions.Left;
        }
        else
        {
            diceTransform.position = dicePos[1].position;
            turnAnnotate.alignment = TextAlignmentOptions.Right;
        }

        if (mode == ChangedPoint.attackMode)
        {
            turnAnnotate.SetText(string.Format("Defending"));
            if (GameController.whoseTurn == GameController.attacker)
            {
                // atkDefStat[1].SetText(string.Format("{0}", players[1].attack));
                dicePos[1].GetComponent<TextMeshProUGUI>().SetText(string.Format("{0}", players[1].attack));
            }
            else // attacked attacking
            {
                // atkDefStat[0].SetText(string.Format("{0}", players[0].attack));
                dicePos[0].GetComponent<TextMeshProUGUI>().SetText(string.Format("{0}", players[0].attack));
            }
        }
        else if (mode == ChangedPoint.defMode)
        {
            turnAnnotate.SetText(string.Format("Attacking"));
            if (GameController.whoseTurn == GameController.gettingAttacked)
            {
                //atkDefStat[1].SetText(string.Format("{0}", players[1].defend));
                int damage = Helper.GetSubtractPlayers(GameController.gettingAttacked - 1, GameController.attacker - 1);
                hpDamage[1].SetText(string.Format("{0}", (damage)));
                dicePos[1].GetComponent<TextMeshProUGUI>().SetText("");
                dicePos[0].GetComponent<TextMeshProUGUI>().SetText("");
                yield return new WaitForSeconds(1.22f);
            }
            else // last turn
            {
                turnAnnotate.SetText(string.Format("Starting Question"));
                turnAnnotate.alignment = TextAlignmentOptions.Center;

                
                // Show Damage to be taken
                int damage = Helper.GetSubtractPlayers(GameController.attacker - 1, GameController.gettingAttacked - 1);
                hpDamage[0].SetText(string.Format("{0}", (damage)));

                yield return new WaitForSeconds(1.22f);

                // Start Questioning from GameController
                StartCoroutine(GameController.Instance.StartQuizCoroutine());
                // return dice to original position
                diceTransform.GetComponent<DiceControl>().ResetPos();
                gameObject.SetActive(false);

            }
        }

        
    }
}
