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
    public enum DiceMode{Move, Attack, Defend, FreePoint, LosePoint, QuizMode}
    public static DiceMode diceMode;

    // Round
    public static int whoseTurn = 1;
    public static bool nextTurn;
    public static int round = 1;
    public static bool gameOver = false; 
    
    // Battle Mode
    public static int attacker;
    public static int gettingAttacked;
    public static bool battleInProgress = false;
    public static int battleTurn = 0;
    
    // Question
    public GameObject quizManager;
    public static QuizAsset.QuestionType questionType;
    public static QuizAsset.QuestionDifficulty questionDifficulty;


    private void Start() {
        Debug.Log("TESTING QUESTION");
        questionType = QuizAsset.QuestionType.Science;
        questionDifficulty = QuizAsset.QuestionDifficulty.Easy;

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
        numPlayer = j;
        diceMode = DiceMode.Move;
    }

    // Changing Dice Mode always result in game waiting for dice input
    public static void ChangeDiceMode(DiceMode newMode) {
        if (diceMode != newMode){
            DiceControl.coroutineAllowed = true;
            nextTurn = false;
            diceMode = newMode;
        }
    }

    public static bool ActionDone(BlockType blockType) {
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
            Debug.Log("Doulberu");
        }
        else if (blockType == BlockType.Quiz)
        {
            Battle(false);
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
            ChangeDiceMode(DiceMode.Defend);
            
            // Switcheroo attack <=> defend for next battle turn
            if (battleTurn == 0)
            {
                whoseTurn = gettingAttacked;
            }
            else if (battleTurn == 1)
            {
                whoseTurn = attacker;
            }

        }
        else if (diceMode == DiceMode.Defend)
        {
            player.playerAttribute.ChangeDefend(diceSideThrown);
            ChangeDiceMode(DiceMode.Attack);
            Debug.Log(battleTurn);
            battleTurn += 1;
            if (battleTurn >= 2)
            {
                battleTurn = 0;
                battleInProgress = false;
            }
        }
        if (!battleInProgress)
        {
            nextTurn = true;    
        }
        
    }
    public static void Battle(bool isPlayer, int opponentNumber = 0) {
        battleInProgress = true;
        if (isPlayer)
        {
            attacker = whoseTurn;
            gettingAttacked = opponentNumber + 1;
            ChangeDiceMode(DiceMode.Attack);
        }
        else
        {
            ChangeDiceMode(DiceMode.Defend);
            // Only Defend for bot battle (correct = win)
            battleTurn = 1;
        }
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
