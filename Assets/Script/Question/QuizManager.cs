using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerTurn;
    [SerializeField] private QuizStore quizStore;
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private Button[] answers;

    private WaitForUIButtons waitAnswerChoices;
    private int currentQuestion;
    private QuizAsset quizAsset = null;
    private PlayerAttribute player0;
    private PlayerAttribute player1;

    [SerializeField] private RectTransform p0Tag, p1Tag;

    private void OnEnable() {
        // Select Category Select Difficulty
        GameController.Instance.RandomizeQuestion();

        foreach (QuizAsset quizGroup in quizStore.quizAssets)
        {
            if (quizGroup.questionType == GameController.questionType & quizGroup.questionDifficulty == GameController.questionDifficulty)
            {
                quizAsset = quizGroup;
                break;
            }
        }
        if (quizAsset == null) 
        {
            quizAsset = quizStore.quizAssets[Random.Range(0, quizStore.quizAssets.Length)];
        }
        player0 = GameController.players_ingame[GameController.attacker - 1].GetComponent<PlayerAttribute>();
        player1 = GameController.players_ingame[GameController.gettingAttacked - 1].GetComponent<PlayerAttribute>();

        // Random pick
        currentQuestion = Random.Range(0, quizAsset.questionAndAnswers.Count);
        // Display Q
        playerTurn.SetText(string.Format("{0}...Answer me this", player0.playerName));
        playerTurn.alignment = TextAlignmentOptions.Left;
        GenerateQuestion();

        // Assign & Display Answer
        SetAnswer();
        waitAnswerChoices = new WaitForUIButtons(answers);
    }

    public IEnumerator Questioning() {
        yield return waitAnswerChoices.Reset();
        AnswerScript starterPressedButton = waitAnswerChoices.PressedButton.GetComponent<AnswerScript>();
        bool attackedCorrect = false;

        // Not NPC
        if (GameController.gettingAttacked != GameController.players_ingame.Length)
        {   
            playerTurn.SetText(string.Format("{0}...Answer me this", player1.playerName));
            playerTurn.alignment = TextAlignmentOptions.Right;
            yield return waitAnswerChoices.Reset();
            AnswerScript AttackedPressedButton = waitAnswerChoices.PressedButton.GetComponent<AnswerScript>();

            // Mean wait for 2nd player to answer first
            // 2nd player will know the if it is correct first
            if (AttackedPressedButton.isCorrect)
            {
                GameController.players_ingame[GameController.gettingAttacked - 1].GetComponent<PlayerAttribute>().ChangeWinPoint(2);
                attackedCorrect = true;
            }
            else // wrong answer
            {
                GameController.Instance.TakeDamage(GameController.gettingAttacked);
            }
            AttackedPressedButton.Answer();

            if (!p1Tag.gameObject.activeSelf)
            {
                p1Tag.gameObject.SetActive(true);
            }
            RectTransform p1 = AttackedPressedButton.GetComponent<RectTransform>();
            p1Tag.position = new Vector3(p1.position.x, p1.position.y);
            p1Tag.GetComponent<TextMeshProUGUI>().SetText(player1.playerName);
            
        }
                
        // Battle attacker
        if (starterPressedButton.isCorrect)
        {
            GameController.players_ingame[GameController.attacker - 1].GetComponent<PlayerAttribute>().ChangeWinPoint(2);
        }
        else // wrong answer
        {
            GameController.Instance.TakeDamage(GameController.attacker);
        }
        starterPressedButton.Answer();
        // Not bot
        if (GameController.gettingAttacked != GameController.players_ingame.Length)
        {
            if (starterPressedButton.isCorrect | attackedCorrect)
            {
                GameController.Instance.StealPoint();
            }
        }
        if (!p0Tag.gameObject.activeSelf)
        {
            p0Tag.gameObject.SetActive(true);
        }

        RectTransform p0 = starterPressedButton.GetComponent<RectTransform>();
        p0Tag.position = new Vector3(p0.position.x, p0.position.y);
        p0Tag.GetComponent<TextMeshProUGUI>().SetText(player0.playerName);

        yield return new WaitForSeconds(2.22f);
        p1Tag.gameObject.SetActive(false);
        p0Tag.gameObject.SetActive(false);
        GameController.Instance.ResetBattle();
        gameObject.SetActive(false);
        
    }

    private void GenerateQuestion() {
        question.SetText(quizAsset.questionAndAnswers[currentQuestion].question);
    }

    private void SetAnswer() {
        //  ***answer_to_shuffle is not shuffling as of now
        string[] answers_to_shuffle = quizAsset.questionAndAnswers[currentQuestion].answers;
        int correctAnswer = quizAsset.questionAndAnswers[currentQuestion].correctAnswer;
        for (int i = 0; i < answers.Length; i++)
        {   
            // Set text at child
            answers[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = answers_to_shuffle[i];
            // Set AnswerScript component at Transform
            if (correctAnswer == i + 1)
            {
                answers[i].transform.GetComponent<AnswerScript>().isCorrect = true;
            }
            else 
            {
                answers[i].transform.GetComponent<AnswerScript>().isCorrect = false;
            }

        }
    }
}
