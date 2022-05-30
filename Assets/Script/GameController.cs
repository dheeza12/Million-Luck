using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockTypeEnum;

public class GameController : MonoBehaviour
{
    public Transform badGuy;
    // Players
    public Transform playersTransform;
    public static int numPlayer = 4;
    public static Transform[] players_ingame = new Transform[5];

    // Dices
    public static int diceSideThrown = 0;
    public int diceSideCheat = 0;
    public enum DiceMode{Move, DoubleMove, Attack, Defend, FreePoint, LosePoint, QuizMode, Revive}
    public static DiceMode diceMode;
    public delegate void DiceModeChange(DiceMode newMode);
    public event DiceModeChange DiceModeChangeEvent;

    // Round
    public static int whoseTurn = 1;
    public static bool nextTurn;
    public static int round = 1;
    public static bool gameOver = false;

    // Battle Mode
    [SerializeField] private GameObject preBattleStatus;
    public static int attacker;
    public static int gettingAttacked;
    public static bool battleInProgress = false;
    public static int battleTurn = 0;
    
    // Question
    [SerializeField] private QuizManager quizManager;
    public static QuizAsset.QuestionType questionType;
    public static QuizAsset.QuestionDifficulty questionDifficulty;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() 
    {
        int j = 0;
        for (int i = 0; i < players_ingame.Length; i++)
        {
            if (i < playersTransform.childCount)
            {
                Transform foundPlayer = playersTransform.GetChild(i);
                
                if (foundPlayer.GetComponent<PlayerAttribute>().play)
                {
                    players_ingame[j] = foundPlayer;
                    j++;
                }
            }
            
        }
        // add bot
        players_ingame[players_ingame.Length - 1] = badGuy;

        numPlayer = j;
        diceMode = DiceMode.Move;
    }


    // Perform 1 time mode change
    public void ChangeDiceMode(DiceMode newMode)
    {
        if (diceMode != newMode)
        {
            DiceControl.coroutineAllowed = true;
            nextTurn = false;
            diceMode = newMode;

            // Fire event for UI to catch
            DiceModeChangeEvent?.Invoke(diceMode);
        }
    }

    public IEnumerator PlayerBattle(int opponentNumber = 0)
    {
        battleInProgress = true;
        // storing attacker and getting attacked player index
        attacker = whoseTurn;
        gettingAttacked = opponentNumber + 1;

        // start battle
        ChangeDiceMode(DiceMode.Attack);
        // if bot
        if (gettingAttacked == players_ingame.Length)
        {
            // act as if player attacked
            battleTurn = 1;
            whoseTurn = gettingAttacked;
            preBattleStatus.SetActive(true);
            yield return StartCoroutine(DiceControl.Instance.RollTheDice());
            ChangeDiceMode(DiceMode.QuizMode);
        }
        preBattleStatus.SetActive(true);
    }

    public void TakeDamage(int playerTurnNo)
    {
        int playerToAttack = whoseTurn;
        if (playerTurnNo == playerToAttack)
        {
            playerToAttack = gettingAttacked;
        }
        
        PlayerAttribute playerDamageTakenAttribute = players_ingame[playerTurnNo - 1].GetComponent<PlayerAttribute>();
        int damageTaken = Helper.GetSubtractPlayers(playerTurnNo - 1, playerToAttack - 1);
        playerDamageTakenAttribute.ChangeHitPoint(damageTaken);
        if (playerDamageTakenAttribute.hp <= 0)
        {
            playerDamageTakenAttribute.ChangeHitPoint(- playerDamageTakenAttribute.hp);
            Debug.Log("A player died");
        }
    }

    public void ResetBattle()
    {
        foreach (PlayerAttribute playerFought in new PlayerAttribute[2]
                { players_ingame[attacker-1].GetComponent<PlayerAttribute>(),
                players_ingame[gettingAttacked - 1].GetComponent<PlayerAttribute>() })
        {
            playerFought.ResetAtkDef();
        }

        gettingAttacked = 0;
        battleInProgress = false;
        battleTurn = 0;
        nextTurn = true;
        
    }

    // Main game control
    public void RollPlayer(int playerToMove)
    {   
        Path player = players_ingame[playerToMove - 1].GetComponent<Path>();

        switch (diceMode)
        {
            case DiceMode.Move:
                player.StartMove();
                break;

            case DiceMode.DoubleMove:
                break;

            case DiceMode.Attack:
                player.playerAttribute.ChangeAttack(diceSideThrown);
                // Switcheroo attack <=> defend for next battle turn

                if (whoseTurn == attacker)
                {
                    whoseTurn = gettingAttacked;
                }
                else
                {
                    whoseTurn = attacker;
                }
                ChangeDiceMode(DiceMode.Defend);
                break;

            case DiceMode.Defend:
                player.playerAttribute.ChangeDefend(diceSideThrown);

                // Proceed to next turn of battle
                battleTurn += 1;

                if (battleTurn >= 2)
                {
                    // StartQuizCoroutine called in BattleSFX.cs
                }
                else
                {
                    ChangeDiceMode(DiceMode.Attack);
                }

                break;

            case DiceMode.FreePoint:
                player.playerAttribute.ChangeScorePoint(diceSideThrown * round);
                break;

            case DiceMode.LosePoint:
                player.playerAttribute.ChangeScorePoint(-diceSideThrown * round / 2);
                break;

            case DiceMode.QuizMode:
                player.playerAttribute.ChangeDefend(diceSideThrown);
                break;

            case DiceMode.Revive:
                if (diceSideThrown >= 5)
                {
                    player.playerAttribute.ChangeHitPoint(+3);
                }
                break;

            default:
                break;
        }
        if (!battleInProgress)
        {
            nextTurn = true;
        }
    }

    // Identify block to change diceMode accordingly
    public bool BlockActionDone(BlockType blockType) {
        switch (blockType)
        {
            case BlockType.Lucky:
                ChangeDiceMode(DiceMode.FreePoint);
                break;
            case BlockType.Unlucky:
                ChangeDiceMode(DiceMode.LosePoint);
                break;
            case BlockType.Quiz:
                // Quiz enter PlayerBattle which change diceMode, which will then cause ChangeDiceMode to occur
                if (diceMode != DiceMode.QuizMode)
                {
                    StartCoroutine(PlayerBattle(players_ingame.Length - 1));
                }
                
                break;
            case BlockType.DoubleMove:
                whoseTurn -= 1;
                break;
            case BlockType.PlayerHome:
                // needed code
                break;
            default:
                break;
        }
        return nextTurn;
    }

    public void RandomizeQuestion() {
        questionType = Helper.RandomEnumValue<QuizAsset.QuestionType>();

        // find difference of two players' attack and defend, the highest difference determind the difficulty
        int difficultyFactor = Mathf.Max(
            Helper.GetSubtractPlayers(attacker - 1, gettingAttacked - 1),
            Helper.GetSubtractPlayers(gettingAttacked - 1, attacker - 1)
                );

        // 0 <= difficultyFactor <= 5
        switch (difficultyFactor)
        {
            case >= 5:
                questionDifficulty = QuizAsset.QuestionDifficulty.Hard;
                break;
            case >= 3:
                questionDifficulty = QuizAsset.QuestionDifficulty.Medium;
                break;
            default:
                questionDifficulty = QuizAsset.QuestionDifficulty.Easy;
                break;
        }
    }

    public IEnumerator StartQuizCoroutine() {
        quizManager.gameObject.SetActive(true);
        yield return StartCoroutine(quizManager.Questioning());
    }

    public void NextTurn(){
        whoseTurn += 1;
        if (whoseTurn > numPlayer)
        {
            whoseTurn = 1;
            round += 1;
        }
        // Normal next turn
        ChangeDiceMode(DiceMode.Move);

        // Check if player died

        // ChangeDiceMode to Revive once
        if (players_ingame[whoseTurn - 1].GetComponent<PlayerAttribute>().hp <= 0)
        {
            ChangeDiceMode(DiceMode.Revive);
        }

        DiceControl.coroutineAllowed = true;
        
    }
}
