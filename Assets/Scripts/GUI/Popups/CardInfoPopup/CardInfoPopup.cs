using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class CardInfoPopup : BasePopup
{
    [SerializeField] TextMeshProUGUI cardName, cardContent, energyText, maxUsedText, cardTypeText;
    [SerializeField] Image cardIcon;

    [SerializeField] PageCardInfo pageSetup, pageAction;
    [SerializeField] Button setupBtn, actionBtn;
    bool isShowPageSetup = true;

    private void OnEnable()
    {
        CheckCreatePageCards();
        ShowPage();
    }

    public bool HasSpawnPages { get; private set; } = false;
    private void CheckCreatePageCards()
    {
        if (HasSpawnPages) return;
        List<CardEnum> setupCards = new();
        List<CardEnum> actionCards = new();
        foreach (var config in Configs.CardConfigs.Values)
        {
            if (!config.IsAbilityCard) setupCards.Add(config.Card);
            else actionCards.Add(config.Card);
        }
        setupCards.Sort(SortCardEnum());
        actionCards.Sort(SortCardEnum());
        pageSetup.ShowCards(this, setupCards);
        pageAction.ShowCards(this, actionCards);
        HasSpawnPages = true;
    }
    private IComparer<CardEnum> SortCardEnum()
    {
        return Comparer<CardEnum>.Create((a, b) => ((int)a).CompareTo((int)b));
    }

    private void ShowPage()
    {
        pageSetup.gameObject.SetActive(isShowPageSetup);
        pageAction.gameObject.SetActive(!isShowPageSetup);
        setupBtn.interactable = !isShowPageSetup;
        actionBtn.interactable = isShowPageSetup;
    }

    public void OnClickShowPage(bool isSetup)
    {
        isShowPageSetup = isSetup;
        ShowPage();
    }

    public void DisplayCardInfo(CardEnum cardEnum)
    {
        var info = Configs.GetCardInfo(cardEnum);
        var config = Configs.GetCardConfig(cardEnum);

        cardName.text = info.DisplayName;
        cardTypeText.text = config.IsAbilityCard ? "-Action Card-" : "-Setup Card-";
        cardIcon.sprite = ResourceProvider.GetCardArt(cardEnum);
        string content = "";
        foreach (var line in info.Contents)
        {
            content += line + "\n";
        }
        cardContent.text = content;

        var costText = $"{config.BaseCost}";
        if (config.EscalatingCost > 0) costText += "+";
        energyText.text = costText;

        maxUsedText.text = config.MaxUsed == -1 ? "" : $"Max uses: {config.MaxUsed}";
    }
    public Action OKAction { get; set; }
    public override void Hide()
    {
        base.Hide();
        OKAction?.Invoke();
        OKAction = null;
    }
}
