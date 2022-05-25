using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "QuizStore", menuName = "Millionaire Luck/QuizStore", order = 0)]
public class QuizStore : ScriptableObject {
    public QuizAsset[] quizAssets;
    
}