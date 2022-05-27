using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AttributeChange;

public class BattleSFX : MonoBehaviour
{
    [Header("UI settings")]
    [SerializeField] private TextMeshProUGUI playerStartName;
    [SerializeField] private TextMeshProUGUI playerAttackedName;
    [SerializeField] private TextMeshProUGUI playerStartStat;
    [SerializeField] private TextMeshProUGUI playerAttackedStat;
    [SerializeField] private TextMeshProUGUI playerStartHpTaken;
    [SerializeField] private TextMeshProUGUI playerAttackedHpTaken;

    [SerializeField] private TextMeshProUGUI turnAnnotate0;
    [SerializeField] private TextMeshProUGUI turnAnnotate1;

    [Header("Dice Transform")]
    [SerializeField] private Transform diceTransform;
    [Header("Dice Position")]
    [SerializeField] private RectTransform dicePos1;
    [SerializeField] private RectTransform dicePos2;

    PlayerAttribute playerStarterAttribute;
    PlayerAttribute playerAttackedAttribute;
    PlayerAttribute[] players = new PlayerAttribute[2];

    IEnumerator Start()
    {
        // wait for 1 frame to let UI be set,
        yield return new WaitForEndOfFrame();
        diceTransform.position = dicePos1.position;

        playerStarterAttribute = GameController.players_ingame[GameController.attacker - 1].GetComponent<PlayerAttribute>();
        playerAttackedAttribute = GameController.players_ingame[GameController.gettingAttacked - 1].GetComponent<PlayerAttribute>();
        players[0] = playerStarterAttribute;
        players[1] = playerAttackedAttribute;
        SetPlayerDisplay();

    }

    private void OnEnable()
    {
        PlayerAttribute.BattleStatChanged += UpdateDisplay;
    }

    private void OnDisable()
    {
        PlayerAttribute.BattleStatChanged -= UpdateDisplay;
    }

    private void SetPlayerDisplay()
    {
        playerStartName.SetText(string.Format("{2} HP:{0}/{1}", players[0].hp, players[0].maxHP, players[0].playerName));

        
        playerAttackedName.SetText(string.Format("{2} HP:{0}/{1}", players[1].hp, players[1].maxHP, players[1].playerName));
        turnAnnotate0.SetText("Attack");
        turnAnnotate1.SetText("Waiting");

    }

    private void UpdateDisplay(ChangedPoint mode)
    {
        if (mode == ChangedPoint.attackMode)
        {
            if (GameController.whoseTurn == GameController.attacker)
            {
                playerStartStat.SetText(string.Format("ATK: {0}", players[0].attack));
                diceTransform.position = dicePos2.position;

                turnAnnotate0.SetText("Waiting");
                turnAnnotate1.SetText("Defend");
            }
            else // attacked attacking
            {
                playerAttackedStat.SetText(string.Format("ATK: {0}", players[1].attack));
                diceTransform.position = dicePos1.position;

                turnAnnotate0.SetText("Defend");
                turnAnnotate1.SetText("Waiting");
            }
        }
        else if (mode == ChangedPoint.defMode)
        {

            if (GameController.whoseTurn == GameController.gettingAttacked)
            {
                playerAttackedStat.SetText(string.Format("DEF: {0}", players[1].defend));

                int damage = players[0].attack - players[1].defend;
                if (damage <= 0)
                {
                    damage = 1;
                }

                playerAttackedHpTaken.SetText(string.Format("taking damage: {0}", (damage)));

                turnAnnotate0.SetText("Waiting");
                turnAnnotate1.SetText("Attack");

            }
            else // last turn
            {
                playerStartStat.SetText(string.Format("DEF: {0}", players[0].defend));

                int damage = players[1].attack - players[0].defend;
                if (damage <= 0)
                {
                    damage = 1;
                }

                playerStartHpTaken.SetText(string.Format("taking damage: {0}", (damage)));

                turnAnnotate0.SetText("Waiting");
                turnAnnotate1.SetText("Waiting");

                Invoke("Fight", 2f);
            }
        }
    }

    private void Fight()
    {
        diceTransform.GetComponent<DiceControl>().ResetPos();
        gameObject.SetActive(false);
    }
}
