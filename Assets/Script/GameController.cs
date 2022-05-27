using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockTypeEnum;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform players;
    [SerializeField] public int cheat_dice = 0;

    // Players
    public static int numPlayer = 4;
    public static Transform[] players_ingame = new Transform[4];

    // Dices
    public static int diceSideThrown = 0;
    public enum DiceMode{Move, DoubleMove, Attack, Defend, FreePoint, LosePoint, QuizMode}
    public static DiceMode diceMode;

    // Round
    public static int whoseTurn = 1;
    public static bool nextTurn;
    public static int round = 1;
    public static bool gameOver = false;

    // Battle Mode
    [SerializeField] private BattleSFX battleStatusDisplay;
    private static BattleSFX battleStatus;

    public static int attacker;
    public static int gettingAttacked;
    public static bool battleInProgress = false;
    public static int battleTurn = 0;
    
    // Question
    [SerializeField] private QuizManager quizManagerAssign;
    private static QuizManager quizManager;
    public static QuizAsset.QuestionType questionType;
    public static QuizAsset.QuestionDifficulty questionDifficulty;

    public delegate void DiceModeChange(DiceMode newMode);
    public static event DiceModeChange DiceModeChangeDel;

    private static GameController instance;


    private void Start() {
        int j = 0;
        for (int i = 0; i < numPlayer; i++)
        {
            Transform foundPlayer = players.Find("Player" + (1+i).ToString());
            if (foundPlayer != null & foundPlayer.GetComponent<PlayerAttribute>().play)
            {         
                players_ingame[j] = foundPlayer;     
                j++;
            }
        }
        // Assign static
        instance = this;
        quizManager = quizManagerAssign;
        battleStatus = battleStatusDisplay;
        numPlayer = j;
        diceMode = DiceMode.Move;
    }

    // Changing Dice Mode always result in game waiting for dice input
    public static void ChangeDiceMode(DiceMode newMode) {
        if (diceMode != newMode){
            DiceControl.coroutineAllowed = true;
            nextTurn = false;
            diceMode = newMode;
            DiceModeChangeDel?.Invoke(diceMode);
            if (diceMode == DiceMode.DoubleMove)
            {
                diceMode = DiceMode.Move;
            }
        }
    }
    
    public static T RandomEnumValue<T> () {
        var v = System.Enum.GetValues(typeof (T));
        return (T) v.GetValue(Random.Range(0, v.Length));
    }

    public static bool BlockActionDone(BlockType blockType) {
        if (blockType == BlockType.Lucky)
        {
            ChangeDiceMode(DiceMode.FreePoint);
        }
        else if (blockType == BlockType.Unlucky)
        {
            ChangeDiceMode(DiceMode.LosePoint);
        }
        else if (blockType == BlockType.DoubleMove)
        {
            // Play one more Move
            whoseTurn -= 1;
        }
        else if (blockType == BlockType.Quiz)
        {
            ChangeDiceMode(DiceMode.QuizMode);
        }
        else if (blockType == BlockType.PlayerHome)
        {
            // Checkpoint
        }
        return nextTurn;
    }
    
    public static void RollPlayer(int playerToMove){
        Path player = players_ingame[playerToMove - 1].GetComponent<Path>();

        Debug.Log("Round: " + round + " Player: " + whoseTurn + " Mode: " + diceMode);
        
        if (diceMode == DiceMode.Move)
        {
            player.StartMove();    
        }
        else if (diceMode == DiceMode.FreePoint)
        {
            player.playerAttribute.ChangeScorePoint(diceSideThrown * round);
        }
        else if (diceMode == DiceMode.LosePoint)
        {
            player.playerAttribute.ChangeScorePoint(-diceSideThrown * round / 2);
        }
        else if (diceMode == DiceMode.Attack)
        {
            player.playerAttribute.ChangeAttack(diceSideThrown);
            
            
            // Switcheroo attack <=> defend for next battle turn
            if (battleTurn == 0)
            {
                whoseTurn = gettingAttacked;
            }
            else if (battleTurn == 1)
            {
                whoseTurn = attacker;
            }
            ChangeDiceMode(DiceMode.Defend);
        }
        else if (diceMode == DiceMode.Defend)
        {
            player.playerAttribute.ChangeDefend(diceSideThrown);
            battleTurn += 1;

            if (battleTurn >= 2)
            {
                // start quiz
                instance.StartCoroutine(instance.StartQuizCoroutine());
            }
            else
            {
                ChangeDiceMode(DiceMode.Attack);
            }
        }
        else if (diceMode == DiceMode.QuizMode)
        {
            player.playerAttribute.ChangeDefend(diceSideThrown);
        }
        if (!battleInProgress)
        {
            nextTurn = true;    
        }
    }

    public static void RandomizeQuestion() {
        Debug.Log("CHANGING CATEGORY QUESTION");
        questionType = RandomEnumValue<QuizAsset.QuestionType>();
        questionDifficulty = QuizAsset.QuestionDifficulty.Easy;
    }

    public IEnumerator StartQuizCoroutine() {
        quizManager.gameObject.SetActive(true);
        yield return quizManager.StartCoroutine(quizManager.Questioning());
        ResetBattle();
    }

    public static void ResetBattle() {
        Debug.Log("RESET");
        gettingAttacked = 0;
        battleInProgress = false;
        battleTurn = 0;
        nextTurn = true;
    }

    public static void Battle(int opponentNumber = 0) {
        battleInProgress = true;
        attacker = whoseTurn;
        gettingAttacked = opponentNumber + 1;
        ChangeDiceMode(DiceMode.Attack);
        battleStatus.gameObject.SetActive(true);
    }
    
    public static void NextTurn(){
        whoseTurn += 1;
        if (whoseTurn > numPlayer)
        {
            whoseTurn = 1;
            round += 1;
        }
        
        ChangeDiceMode(DiceMode.Move);
        DiceControl.coroutineAllowed = true;
        
    }
}
