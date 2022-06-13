using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance;

    private TextMeshProUGUI tooltipText;
    private RectTransform bgRect;
    private RectTransform parentRect;
    [SerializeField] private RectTransform canvasRect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        parentRect = GetComponent<RectTransform>();
        bgRect = transform.Find("bg").GetComponent<RectTransform>();
        tooltipText = transform.Find("text").GetComponent<TextMeshProUGUI>();

        HideTooltip();
    }

    private void SetText(string newTooltipText)
    {
        tooltipText.SetText(newTooltipText);
        tooltipText.ForceMeshUpdate();

        Vector2 textSize = tooltipText.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(tooltipText.margin.x * 2, tooltipText.margin.y * 2);
        bgRect.sizeDelta = textSize + paddingSize;

    }

    private void UpdatePos()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRect.localScale.x;
        if (anchoredPosition.x + bgRect.rect.width > canvasRect.rect.width)
        {   // Tooltip left screen on right side
            anchoredPosition.x = canvasRect.rect.width - bgRect.rect.width;
        }
        if (anchoredPosition.y + bgRect.rect.height > canvasRect.rect.height)
        {   // left on top
            anchoredPosition.y = canvasRect.rect.height - bgRect.rect.height;
        }
        parentRect.anchoredPosition = anchoredPosition;
    }

    public void ShowTooltip(string newString)
    {
        gameObject.SetActive(true);
        UpdatePos();
        SetText(newString);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
