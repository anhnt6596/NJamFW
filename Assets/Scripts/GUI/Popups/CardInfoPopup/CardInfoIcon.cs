using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoIcon : MonoBehaviour
{
    [SerializeField] Image icon;

    CardEnum card;
    System.Action<CardEnum> clickCB;
    public void SetData(CardEnum card, System.Action<CardEnum> clickCB)
    {
        icon.sprite = ResourceProvider.GetCardArt(card);
        this.card = card;
        this.clickCB = clickCB;
    }

    public void OnClick() => clickCB?.Invoke(card);
}
