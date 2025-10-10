using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class CardInfoPopup : BasePopup
{
    [SerializeField] TextMeshProUGUI cardName, cardContent;
    [SerializeField] Image cardIcon;

    public void SetCardInfo(CardEnum cardEnum)
    {
        var cardInfo = Configs.GetCardInfo(cardEnum);
        cardName.text = cardInfo.DisplayName;
        cardIcon.sprite = ResourceProvider.GetCardArt(cardEnum);
        string content = "";
        foreach (var line in cardInfo.Contents)
        {
            content += line + "\n";
        }
        cardContent.text = content;
    }
    public Action OKAction { get; set; }
    public override void Hide()
    {
        base.Hide();
        OKAction?.Invoke();
    }
}
