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

    [SerializeField] private TextMeshProUGUI[] atkDefStat;

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

        // Getting players in a turn
        players[0] = GameController.players_ingame[GameController.attacker - 1].GetComponent<PlayerAttribute>();
        players[1] = GameController.players_ingame[GameController.gettingAttacked - 1].GetComponent<PlayerAttribute>();


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
            names[i].SetText(string.Format("{2}\nHP:{0}/{1}", players[i].hp, players[i].maxHP, players[i].playerName));
            atkDefStat[i].SetText(string.Format(""));
            dicePos[i].gameObject.GetComponent<TextMeshProUGUI>().SetText("");
        }

    }

    private void StartUpdateDisplay(ChangedPoint mode)
    {
        StartCoroutine(UpdateDisplay(mode));
    }

    private IEnumerator UpdateDisplay(ChangedPoint mode)
    {
        yield return new WaitForSeconds(0.44f);
        hpDamage[0].SetText(string.Format("taking damage: {0}", (Helper.GetSubtractPlayers(GameController.attacker - 1, GameController.gettingAttacked - 1))));
        hpDamage[1].SetText(string.Format("taking damage: {0}", (Helper.GetSubtractPlayers(GameController.gettingAttacked - 1, GameController.attacker - 1))));
        if (GameController.whoseTurn == GameController.attacker)
        {
            diceTransform.position = dicePos[0].position;
        }
        else
        {
            diceTransform.position = dicePos[1].position;
        }

        if (mode == ChangedPoint.attackMode)
        {
            turnAnnotate.SetText(string.Format("{0} : Defending", GameController.players_ingame[GameController.whoseTurn - 1].GetComponent<PlayerAttribute>().playerName));
            if (GameController.whoseTurn == GameController.attacker)
            {
                atkDefStat[1].SetText(string.Format("{0}", players[1].attack));
            }
            else // attacked attacking
            {
                atkDefStat[0].SetText(string.Format("{0}", players[0].attack));
            }
        }
        else if (mode == ChangedPoint.defMode)
        {
            turnAnnotate.SetText(string.Format("{0} : Attacking", GameController.players_ingame[GameController.whoseTurn - 1].GetComponent<PlayerAttribute>().playerName));
            if (GameController.whoseTurn == GameController.gettingAttacked)
            {
                atkDefStat[1].SetText(string.Format("{0}", players[1].defend));
                int damage = Helper.GetSubtractPlayers(GameController.gettingAttacked - 1, GameController.attacker - 1);
                hpDamage[1].SetText(string.Format("taking damage: {0}", (damage)));

            }
            else // last turn
            {
                turnAnnotate.SetText(string.Format("Starting Question"));
                atkDefStat[0].SetText(string.Format("{0}", players[0].defend));                

                // return dice to original position
                diceTransform.GetComponent<DiceControl>().ResetPos();
                // Show Damage to be taken
                dicePos[0].gameObject.GetComponent<TextMeshProUGUI>().SetText(hpDamage[0].text);
                dicePos[1].gameObject.GetComponent<TextMeshProUGUI>().SetText(hpDamage[1].text);
                
                yield return new WaitForSeconds(2.66f);

                // Start Questioning from GameController
                StartCoroutine(GameController.Instance.StartQuizCoroutine());
                gameObject.SetActive(false);

            }
        }
        
    }
}
