using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceControl : MonoBehaviour
{
    private Sprite[] diceSides;
    private SpriteRenderer rend;
    public static bool coroutineAllowed = true;

    public static DiceControl Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start() {
        rend = GetComponent<SpriteRenderer>();
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
        if (!GameController.gameOver && coroutineAllowed)
        {
            StartCoroutine("RollTheDice");
        }
    }

    public void ResetPos()
    {
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public IEnumerator RollTheDice(){
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
        int cheat = GameController.Instance.diceSideCheat;
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
        GameController.Instance.RollPlayer(GameController.whoseTurn);

    }

}
