using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private QuizStore quizStore;
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private Button[] answers;
    private WaitForUIButtons waitAnswerChoices;
    private int currentQuestion;
    private QuizAsset quizAsset = null;

    private void OnEnable() {
        // Select Category
        GameController.RandomizeQuestion();
        // Select Difficulty

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

        // Random pick
        currentQuestion = Random.Range(0, quizAsset.questionAndAnswers.Count);
        // Display Q
        GenerateQuestion();

        // Assign & Display Answer
        SetAnswer();
        waitAnswerChoices = new WaitForUIButtons(answers);
    }

    public IEnumerator Questioning() {
        Debug.Log("Start QUESTIONING");
        yield return waitAnswerChoices.Reset();
        AnswerScript pressedButton = waitAnswerChoices.PressedButton.GetComponent<AnswerScript>();
        if (pressedButton.isCorrect)
        {
            
        }
        pressedButton.Answer();
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
