using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    #region Game Events
    public static event Action OnCardRolled;
    public static event Action<SelectionCard> OnCardLocked;
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
        RollSelectionCards();
    }

    public void PayToRerollCards()
    {
        if (State.energy < gameConfig.RerollCardCost) return;
        IncreaseEnergy(-gameConfig.RerollCardCost);
        RollSelectionCards();
    }

    public SelectionCard GetLockedCard()
    {
        return SelectionCards.Cards.FirstOrDefault(c => c.isLocked);
    }

    public void DoLockCard(SelectionCard card)
    {
        if (GetLockedCard() != null) return;

        var found = SelectionCards.Cards.First(c => c == card);
        found.isLocked = true;
        OnCardLocked?.Invoke(found);
    }

    private void RollSelectionCards()
    {
        SelectionCards.Roll();
        OnCardRolled?.Invoke();
    }

    public void Update()
    {
        if (IsRunning) UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        IncreaseEnergy(gameConfig.BaseEnergyPerSec * Time.deltaTime);
    }

    private void IncreaseEnergy(float value)
    {
        State.energy = Mathf.Clamp(State.energy + value, 0, gameConfig.MaxEnergy);
    }
}
