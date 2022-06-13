using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttributeChange;
using UnityEngine.UI;


public class PointsPopper : MonoBehaviour
{
    [SerializeField] private Transform spriteParent;
    private RectTransform rect;
    private Sprite[] questSprite;

    public static PointsPopper Instance { get; private set; }

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

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        questSprite = Resources.LoadAll<Sprite>("Currency/");
    }

    public void SpawnPopping(ChangedPoint changedPoint, Transform target, int amount = 7)
    {
        int amount_count = Mathf.Abs(amount);
        // make sure it's not exceeded
        if (amount_count > spriteParent.childCount)
        {
            amount_count = spriteParent.childCount;
        }

        for (int i = 0; i < amount_count; i++)
        { 
            if (!spriteParent.GetChild(i).gameObject.activeSelf)
            {
                spriteParent.GetChild(i).gameObject.SetActive(true);
            }
            // Multiple PlayerUIComponent call
            else if (spriteParent.GetChild(i).gameObject.activeSelf) 
            {
                amount_count++;
                if (amount_count > spriteParent.childCount)
                {
                    amount_count = spriteParent.childCount;
                }
                continue;
            }

            // Switch sprites
            switch (changedPoint)
            {
                case ChangedPoint.hpChanged:
                    spriteParent.GetChild(i).GetComponent<Image>().sprite = questSprite[2];
                    break;
                case ChangedPoint.luckChanged:
                    spriteParent.GetChild(i).GetComponent<Image>().sprite = questSprite[1];
                    break;
                case ChangedPoint.winChanged:
                    spriteParent.GetChild(i).GetComponent<Image>().sprite = questSprite[0];
                    break;
            }

            Vector3 randomAround = Random.insideUnitCircle * 50;

            if (amount > 0)
            {
                spriteParent.GetChild(i).position = rect.position + randomAround;
                spriteParent.GetChild(i).GetComponent<PointMove>().MoveToward(target.position);
            }
            else if (amount < 0)
            {
                spriteParent.GetChild(i).position = target.position + randomAround;
                spriteParent.GetChild(i).GetComponent<PointMove>().MoveToward(rect.position);
            }
        }
    }
}
