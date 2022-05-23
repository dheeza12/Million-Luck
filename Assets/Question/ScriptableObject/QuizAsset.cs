using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizAsset", menuName = "Millionaire Luck/QuizAsset", order = 0)]
public class QuizAsset : ScriptableObject {
    public List<QuestionAndAnswer> questionAndAnswers;
    public enum QuestionType {Math, Science, History, Trivial};
    public enum QuestionDifficulty {Easy, Medium, Hard};
    public QuestionType questionType;
    public QuestionDifficulty questionDifficulty;

}
