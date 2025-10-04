using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    #region Game Events

    public static event Action<InputStateEnum> OnInputStateChanged;
    public static event Action OnCardsRolled;
    public static event Action OnCardLocked;

    #endregion Game Events

    private CardRoller CardRoller { get; }
    public Game(int level, GameState state)
    {
        Level = level;
        State = state;
        GameConfig = Configs.GamePlay;
        LevelConfig = Configs.GetLevelConfig(level);
        CardRoller = new CardRoller(this);
        CurrentWave = -1;
    }
    public GameState State { get; private set; }
    private GamePlayConfig GameConfig { get; set; }
    private LevelConfig LevelConfig { get; set; }
    public int Level { get; }
    public int CurrentWave { get; private set; }
    public IGamePlay GamePlay { get; set; }
    private InputStateEnum _inputStateEnum;
    public InputStateEnum InputStateEnum
    {
        get => _inputStateEnum;
        private set
        {
            if (_inputStateEnum != value)
            {
                _inputStateEnum = value;
                OnInputStateChanged?.Invoke(_inputStateEnum);
            }
        }
    }
    public CardEnum PlayingCard { get; private set; } = CardEnum.None;

    public bool IsRunning { get; private set; } = false;

    public void StartGame()
    {
        IsRunning = true;
        RollCards(true);
        CheckRunNextWave();
    }

    public void DoPayReroll()
    {
        if (State.freeRoll > 0)
        {
            State.freeRoll--;
            RollCards();
            return;
        }

        if (State.energy < GameConfig.RerollCardCost) return;
        IncreaseEnergy(-GameConfig.RerollCardCost);

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
        var inputState = Configs.GetCardConfig(cardEnum).ApplySellectedEffect(this);
        State.selectedCards.Add(cardEnum);
        if (inputState == InputStateEnum.None || inputState == InputStateEnum.SelectingCard)
        {
            RollCards(true);
        }
        else
        {
            PlayingCard = cardEnum;
            InputStateEnum = inputState;
        }
    }

    private void RollCards(bool isAutoRoll = false)
    {
        CardRoller.Roll();

        if (isAutoRoll) State.autoRolled++;
        else
        {
            IncreaseEnergy(GetRollEnergy());
            State.proactiveRolled++;
        }

        InputStateEnum = InputStateEnum.SelectingCard;
        OnCardsRolled?.Invoke();
    }

    private float GetRollEnergy()
    {
        float baseRollEnergy = 0;
        foreach (var modifier in GetAllModifiers<IRollEnergyModifier>())
        {
            baseRollEnergy = modifier.ModifyRollEnergy(baseRollEnergy);
        }
        return baseRollEnergy;
    }

    private void CheckRunNextWave()
    {
        CurrentWave++;
        if (CurrentWave > LevelConfig.WaveCount)
        {
            // end game
        }
        else
        {
            var waveConfig = LevelConfig.GetWaveConfig(CurrentWave);
            GamePlay?.OnNewWaveStarted(waveConfig);
        }
    }

    public IEnumerable<T> GetAllModifiers<T>() where T: IModifier
    {
        foreach (var card in State.selectedCards)
        {
            var config = Configs.GetCardConfig(card);
            if (config is T modifier) yield return modifier;
        }
    }

    public void Update()
    {
        if (IsRunning) UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        IncreaseEnergy(GameConfig.BaseEnergyPerSec * Time.deltaTime);
    }

    public void IncreaseEnergy(float value)
    {
        State.energy = Mathf.Clamp(State.energy + value, 0, GameConfig.MaxEnergy);
    }

    #region Game Actions
    public void CastLightnings(int times, Damage damage)
    {
        GamePlay?.CastGameLightnings(times, damage);
    }
    public void DoFrozenAllEnemies(float duration)
    {
        GamePlay?.OnAllEnemiesFroze(duration);
    }
    
    public void DoReverseAllEnemies(float duration)
    {
        GamePlay.OnAllEnemiesReversed(duration);
    }

    public void DropBomb(Vector3 position)
    {
        if (InputStateEnum != InputStateEnum.PlayCard) return;
        // hardcode goi config, sau nay lay dmg va radius tu modifier
        var config = ((BombCardConfig)Configs.GetCardConfig(CardEnum.Bomb));
        GamePlay.OnBombDropped(position, config.Damage, config.Radius);
        RollCards(true);
    }

    public void PlaceTower(int placeIndex, TowerEnum tower)
    {
        Debug.Log($"Place Tower {tower}");
        GamePlay?.PlaceTower(placeIndex, tower);
        RollCards(true);
    }
    #endregion
}
