using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private int whoseTurn = 1;
    private int numTurn = 0;
    private bool coroutineAllowed = true;

    private void Start() {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];
    }

    private void OnMouseDown() {
        if (!GameControl.gameOver && coroutineAllowed)
        {
            StartCoroutine("RollTheDice");
        }
    }

    private IEnumerator RollTheDice(){
        coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i < 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        GameControl.diceSideThrown = randomDiceSide + 1;
        

        numTurn++;


        if (whoseTurn == 1)
        {
            GameControl.MovePlayer(1);
        }
        else if (whoseTurn == 2)
        {
            GameControl.MovePlayer(2);
        }
        else if (whoseTurn == 3)
        {
            GameControl.MovePlayer(3);
        }
        else if (whoseTurn == 4)
        {
            GameControl.MovePlayer(4);
        }
        
        // Debug.Log("Total Players: " + GameControl.numPlayer);
        // Debug.Log("Player: " + whoseTurn);

        whoseTurn += 1;
        
        if (whoseTurn > GameControl.numPlayer)
        {
            whoseTurn = 1;
        }

        coroutineAllowed = true;

    }

}
