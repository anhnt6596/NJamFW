using System;
using System.Collections.Generic;
using System.Linq;

public class SelectionCards
{
    Game Game { get; }
    List<CardEnum> RollableCards { get; }
    public List<SelectionCard> Cards { get; }
    public SelectionCards(Game game)
    {
        Game = game;
        RollableCards = GetRollableCard();
        Cards = new();
    }

    public void Roll()
    {
        var newCards = RollCards(Cards);
        Cards.Clear();
        Cards.AddRange(newCards);
    }

    private List<CardEnum> GetRollableCard()
    {
        List<CardEnum> result = new();
        var cardConfigs = Configs.CardConfigs;
        foreach (var card in cardConfigs.Keys) result.Add(card);
        return result;
    }

    private List<SelectionCard> RollCards(List<SelectionCard> last)
    {
        if (last == null) last = new List<SelectionCard>();
        List<SelectionCard> result = new();
        List<CardEnum> lockedCards = last.Where(c => c.isLocked).Select(c => c.cardType).ToList();
        var rollableCards = RollableCards.Where(c => !lockedCards.Contains(c)).ToList();
        var newListCard = RandomHelper.RandomUniqueList(rollableCards, Configs.GamePlay.SelectionCardNumber);
        for (int i = 0; i < newListCard.Count; i++)
        {
            SelectionCard lastCardInThisIndex = null;
            if (last.Count > i) lastCardInThisIndex = last[i];
            if (lastCardInThisIndex != null && lastCardInThisIndex.isLocked)
            {
                result.Add(lastCardInThisIndex);
            }
            else
            {
                result.Add(new SelectionCard()
                {
                    cardType = newListCard[i],
                    isLocked = false,
                    isSelected = false
                });
            }
        }
        return result;
    }
}