using System;
using System.Collections.Generic;
using System.Linq;

public class CardRoller
{
    Game Game { get; }
    List<CardEnum> RollableCards { get; }
    public List<CardEnum> Cards => Game.State.cards;
    public List<int> LockedIdxs => Game.State.lockedCardIdxs;
    public CardRoller(Game game)
    {
        Game = game;
        RollableCards = GetRollableCard();
    }

    public void Roll()
    {
        var newCards = RollCards(Cards, LockedIdxs);
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

    private List<CardEnum> RollCards(List<CardEnum> lastRoll, List<int> lockedCardIdxs)
    {
        List<CardEnum> result = new();
        // 
        List<CardEnum> lockedCards = new List<CardEnum>();
        // them logic roll sau k ra card roll truoc sau, bay h chi ignore locked card thoi
        for (int i = 0; i < lastRoll.Count; i++)
        {
            if (lockedCardIdxs.Contains(i))
            {
                lockedCards.Add(lastRoll[i]);
            }
        }

        var rollableCards = RollableCards
            .Where(c => !lockedCards.Contains(c))
            //.Where(c => !lastRoll.Contains(c))
            .Where(c => Configs.GetCardConfig(c).CanBeRoll(Game))
            .ToList();
        var newListCard = RandomHelper.RandomUniqueList(rollableCards, Configs.GamePlay.SelectionCardNumber);
        for (int i = 0; i < newListCard.Count; i++)
        {
            if (lockedCardIdxs.Contains(i))
            {
                result.Add(lastRoll[i]);
            }
            else
            {
                result.Add(newListCard[i]);
            }
        }
        return result;
    }
}