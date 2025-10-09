using System;
using System.Collections.Generic;
using System.Linq;

public class CardRoller
{
    Game Game { get; }
    //List<CardEnum> RollableCards { get; }
    List<CardEnum> RollableAbilityCards { get; }
    List<CardEnum> RollablePrepareCards { get; }
    public List<CardEnum> Cards => Game.State.cards;
    public List<int> LockedIdxs => Game.State.lockedCardIdxs;
    public CardRoller(Game game)
    {
        Game = game;
        //RollableCards = GetRollableCard();
        RollableAbilityCards = GetRollableAbilityCards();
        RollablePrepareCards = GetRollablePrepareCards();
    }

    public void Roll()
    {
        // hard code 1st and 2nd roll
        List<CardEnum> newCards;
        if (false) newCards = new List<CardEnum>() {
            CardEnum.Troop,
            CardEnum.Troop,
            CardEnum.Troop,
        };
        else if (Game.State.totalRolled == 0) newCards = new List<CardEnum>() {
            CardEnum.ArcherTower,
            CardEnum.MageTower,
            CardEnum.ArtilleryTower,
        };
        else if (Game.State.totalRolled == 1 && false) newCards = new List<CardEnum>()
        {
            CardEnum.Lightning,
            CardEnum.Troop,
            CardEnum.Bomb,
        };
        else newCards = RollCards(Cards, LockedIdxs);
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

    private List<CardEnum> GetRollablePrepareCards()
    {
        List<CardEnum> result = new();
        var cardConfigs = Configs.CardConfigs;
        foreach (var card in cardConfigs.Keys)
        {
            if (!cardConfigs[card].IsAbilityCard) result.Add(card);
        }
        return result;
    }

    private List<CardEnum> GetRollableAbilityCards()
    {
        List<CardEnum> result = new();
        var cardConfigs = Configs.CardConfigs;
        foreach (var card in cardConfigs.Keys)
        {
            if (cardConfigs[card].IsAbilityCard) result.Add(card);
        }
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
        var pool = Game.TurnPhase == TurnPhaseEnum.Prepare ? RollablePrepareCards : RollableAbilityCards;
        var rollableCards = pool
            .Where(c => !lockedCards.Contains(c))
            //.Where(c => !lastRoll.Contains(c)) // add to new roll do not contain last roll
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