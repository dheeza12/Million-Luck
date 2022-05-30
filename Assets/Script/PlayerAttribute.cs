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
    [HideInInspector] public int hp;
    
    [Header("Win Conditions")]
    [SerializeField] public int level = 0;
    [SerializeField] public int score = 0;
    [SerializeField] public int win = 0;

    public int attack;
    public int defend;

    // delegate events
    public delegate void PlayerStart();
    public event PlayerStart NotInGame;

    public delegate void PlayerAction(ChangedPoint mode, int hp, int score, int win);
    public event PlayerAction StatChanged;

    public delegate void BattleStat(ChangedPoint mode);
    public static event BattleStat BattleStatChanged;

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

    public void ResetAtkDef()
    {
        attack = 0;
        defend = 0;
    }

    public void Refresh() {
        hp = maxHP;
        score = 0;
        win = 0;
        StatChanged?.Invoke(ChangedPoint.ResetChanged, hp, score, win);
    }

    public void ChangeHitPoint(int hit) {
        hp += hit;
        Debug.Log(hit);
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
        BattleStatChanged?.Invoke(ChangedPoint.attackMode);
    }

    public void ChangeDefend(int def) {
        defend = def;
        BattleStatChanged?.Invoke(ChangedPoint.defMode);
    }    
    
}
