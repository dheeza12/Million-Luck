using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceControl : MonoBehaviour
{
    private Sprite[] diceSides;
    private SpriteRenderer rend;
    public static bool coroutineAllowed = true;
    [SerializeField] private GameObject cheater;
    private GameController gameController;
    

    private void Start() {
        rend = GetComponent<SpriteRenderer>();
        gameController = cheater.GetComponent<GameController>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];
    }

    private void OnMouseDown() {
        if (!GameController.gameOver && coroutineAllowed)
        {
            StartCoroutine("RollTheDice");
        }
    }

    public void RollAnyway() {
        StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice(){
        // Disable Dice Roll after rolling
        DiceControl.coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i < 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        // Cheat Mode
        int cheat = gameController.cheat_dice;
        if (cheat > 0)
        {
            GameController.diceSideThrown = cheat;  
            if (cheat <= 6){
                rend.sprite = diceSides[cheat - 1]; 
            }
        }
        else
        {
            GameController.diceSideThrown = randomDiceSide + 1;    
        }

        // Action
        GameController.RollPlayer(GameController.whoseTurn);

    }

}
