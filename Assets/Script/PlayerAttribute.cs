using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttributeChange;

public class PlayerAttribute : MonoBehaviour
{
    [SerializeField] public bool play;
    [SerializeField] public string playerName;

    [Header("Player Attributes")]
    [SerializeField] public int maxHP;
    private int hp;
    
    [SerializeField] private int score = 0;
    [SerializeField] private int win = 0;

    
    private int attack;
    private int defend;


    public delegate void PlayerStart();
    public event PlayerStart NotInGame;

    public delegate void PlayerAction(ChangedPoint mode, int hp, int score, int win);
    public event PlayerAction StatChanged;

    private void Start() {
        // In play, Shoot event to let UI update
        if (play)
        {
            // Active
            Refresh();
        }
        else
        {
            // if NotInGame != null OR NotInGame?.Invoke()
            NotInGame();   
            gameObject.SetActive(false);
        }
    }

    public void Refresh() {
        hp = maxHP;
        score = 0;
        win = 0;
        StatChanged?.Invoke(ChangedPoint.ResetChanged, hp, score, win);
    }

    public void ChangeHitPoint(int hit) {
        hp += hit;
        StatChanged?.Invoke(ChangedPoint.hpChanged, hp, score, win);
    }
    public void ChangeScorePoint(int hit) {
        score += hit;
        if (score < 0)
        {
            score = 0;
        }
        StatChanged?.Invoke(ChangedPoint.scoreChanged, hp, score, win);
    }
    public void ChangeWinPoint(int hit) {
        win += hit;
        StatChanged?.Invoke(ChangedPoint.winChanged, hp, score, win);
    }

    public void ChangeAttack(int atk) {
        attack = atk;
        Debug.Log(attack);
    }

    public void ChangeDefend(int def) {
        defend = def;
        Debug.Log(def);
    }    
    
}
