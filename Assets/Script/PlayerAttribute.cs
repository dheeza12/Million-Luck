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

    [SerializeField] public int level = 0;
    [SerializeField] public int score = 0;
    [SerializeField] public int win = 0;
    public enum WinCondition { winWin, ScoreWin }
    public WinCondition winCondition;

    [Header("Battle Status")]
    public int attack;
    public int defend;

    // delegate events
    public delegate void PlayerStart();
    public event PlayerStart NotInGame;

    public delegate void PlayerAction(ChangedPoint mode, int change);
    public event PlayerAction StatChanged;

    public delegate void BattleStat(ChangedPoint mode);
    public static event BattleStat BattleStatChanged;

    public delegate void LevelChanged();
    public static event LevelChanged LevelSelect;

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
        StatChanged?.Invoke(ChangedPoint.ResetChanged, 0);
    }

    public void ChangeLevel(int hit)
    {
        level += hit;
        LevelSelect?.Invoke();
    }

    public void ChangeHitPoint(int hit) {
        hp += hit;
        StatChanged?.Invoke(ChangedPoint.hpChanged, hit);
    }
    public void ChangeScorePoint(int hit) {
        score += hit;
        if (score < 0)
        {
            score = 0;
        }
        StatChanged?.Invoke(ChangedPoint.luckChanged, hit);
    }
    public void ChangeWinPoint(int hit) {
        win += hit;
        StatChanged?.Invoke(ChangedPoint.winChanged, hit);
    }

    public void ChangeAttack(int atk) {
        attack = atk;
        BattleStatChanged?.Invoke(ChangedPoint.attackMode);
    }

    public void ChangeDefend(int def) {
        defend = def;
        BattleStatChanged?.Invoke(ChangedPoint.defMode);
    }    

    public void ChangeWinCondition(WinCondition newWinCondition)
    {
        winCondition = newWinCondition;
        LevelSelect?.Invoke();
    }


}
