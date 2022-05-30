using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static T RandomEnumValue<T>()
    {
        var v = System.Enum.GetValues(typeof(T));
        return (T)v.GetValue(Random.Range(0, v.Length));
    }

    public static int GetSubtractPlayers(int p1, int p2)
    {
        int damageTake = GameController.players_ingame[p1].GetComponent<PlayerAttribute>().defend -
            GameController.players_ingame[p2].GetComponent<PlayerAttribute>().attack;
        if (damageTake >= 0)
        {
            damageTake = -1;
        }
        return damageTake;
    }
}
