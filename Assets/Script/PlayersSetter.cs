using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersSetter : MonoBehaviour
{
    [SerializeField] private PlayerAttribute[] playerAttributes;
    [SerializeField] private SpriteRenderer[] playerSpriteRend;

    private void Awake()
    {
        for (int i = 0; i < playerAttributes.Length; i++)
        {
            playerAttributes[i].playerName = PlayersSetting.instance.playersName[i].text;
            playerAttributes[i].play = PlayersSetting.instance.inGame[i];
            playerSpriteRend[i].color = PlayersSetting.instance.playersSprite[i].color;
            playerSpriteRend[i].sprite = PlayersSetting.instance.playersSprite[i].sprite;

        }
    }

}
