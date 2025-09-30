using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    #region Game Events
    public static event Action OnCardsRolled;
    public static event Action OnCardLocked;
    #endregion Game Events

    public SelectionCards SelectionCards { get; }
    public Game(int level, GameState state)
    {
        Level = level;
        State = state;
        gameConfig = Configs.GamePlay;
        SelectionCards = new SelectionCards(this);
    }
    public GameState State { get; private set; }
    private GamePlayConfig gameConfig;
    public int Level { get; }
    public bool IsRunning { get; private set; } = false;

    public void StartGame()
    {
        IsRunning = true;
        RollCards();
    }

    public void DoPayReroll()
    {
        if (State.freeRoll > 0)
        {
            State.freeRoll--;
            RollCards();
            return;
        }

        if (State.energy < gameConfig.RerollCardCost) return;
        IncreaseEnergy(-gameConfig.RerollCardCost);

        RollCards();
    }

    public bool IsMaxLockedCard => State.lockedCardIdxs.Count >= Configs.GamePlay.MaxLockedCard;

    public void DoLockCard(int cardIdx)
    {
        if (IsMaxLockedCard) return;
        if (State.lockedCardIdxs.Contains(cardIdx)) return;

        State.lockedCardIdxs.Add(cardIdx);
        OnCardLocked?.Invoke();
    }

    public float GetCardCost(CardEnum card)
    {
        // them so luot da su dung card trong game vao day
        var usedTime = State.selectedCards.Count(c => c == card);
        return Configs.GetCardConfig(card).GetCost(this);
    }

    public void DoSelectCard(int cardIdx)
    {
        var cardEnum = State.cards[cardIdx];
        var energyCost = GetCardCost(cardEnum);

        if (State.energy < energyCost) return;

        IncreaseEnergy(-energyCost);

        if (State.lockedCardIdxs.Contains(cardIdx)) State.lockedCardIdxs.Remove(cardIdx);
        State.selectingCardIdx = cardIdx;

        // Do Card Action
        Configs.GetCardConfig(cardEnum).ApplySellectedEffect(this);

        // Test Skip qua phase action cua card, reroll luon
        State.selectedCards.Add(cardEnum);
        RollCards();
    }

    private void RollCards()
    {
        SelectionCards.Roll();
        OnCardsRolled?.Invoke();
    }

    public void Update()
    {
        if (IsRunning) UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        IncreaseEnergy(gameConfig.BaseEnergyPerSec * Time.deltaTime);
    }

    public void IncreaseEnergy(float value)
    {
        State.energy = Mathf.Clamp(State.energy + value, 0, gameConfig.MaxEnergy);
    }
}
