using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Game
{
    #region Game Events

    public static event Action<InputStateEnum> OnInputStateChanged;
    public static event Action OnCardsRolled;
    public static event Action OnCardLocked;
    public static event Action<int, TurnPhaseEnum> OnPhaseChanged;
    public static event Action<int, int> HealthChanged;

    #endregion Game Events

    private CardRoller CardRoller { get; }
    public Game(int level, GameState state)
    {
        Level = level;
        State = state;
        GameConfig = Configs.GamePlay;
        LevelConfig = Configs.GetLevelConfig(level);
        CardRoller = new CardRoller(this);
        CurrentTurn = -1;
        InputStateEnum = InputStateEnum.None;
    }
    public GameState State { get; private set; }
    private GamePlayConfig GameConfig { get; set; }
    private LevelConfig LevelConfig { get; set; }
    public int Level { get; }

    #region Turn Info
    private TurnPhaseEnum _turnPhase;
    public TurnPhaseEnum TurnPhase
    {
        get => _turnPhase;
        private set
        {
            _turnPhase = value;
            OnPhaseChanged?.Invoke(CurrentTurn, _turnPhase);
        }
    }
    public int CurrentTurn { get; private set; }
    public bool IsLastTurn => CurrentTurn >= LevelConfig.TurnCount - 1;
    private void StartNewTurn()
    {
        CurrentTurn++;
        var turnConfig = LevelConfig.GetTurnConfig(CurrentTurn);
        IncreaseEnergy(turnConfig.TurnEnergyGain);
        State.freeRoll += turnConfig.TurnFreeRollGain;
        TurnPhase = TurnPhaseEnum.Prepare;
        RollCards(true);
    }

    public void ReadyForTurn()
    {
        TurnPhase = TurnPhaseEnum.Combat;
        CheckRunNextWave();
        RollCards(true);
    }

    public void CompleteTurn()
    {
        if (!IsLastTurn) StartNewTurn();
        else Win();
    }

    #endregion Turn Info

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
        State.InitialState();
        StartNewTurn();
    }

    public void TakeDamage(int value)
    {
        if (State.baseHealth <= 0) return;
        var last = State.baseHealth;
        State.baseHealth -= value;
        HealthChanged?.Invoke(State.baseHealth, last);
        if (State.baseHealth <= 0) App.Get<GameManager>().GameLose();
    }

    public void Heal(int value)
    {
        if (State.baseHealth >= Configs.GamePlay.BaseHealth) return;
        var last = State.baseHealth;
        State.baseHealth = Mathf.Min(Configs.GamePlay.BaseHealth, State.baseHealth + value);
        HealthChanged?.Invoke(State.baseHealth, last);
    }

    public void Win() => App.Get<GameManager>().GameWin();

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

    public double GetCardCost(CardEnum card)
    {
        // them so luot da su dung card trong game vao day
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
        if (inputState == InputStateEnum.SelectingCard)
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

    private double GetRollEnergy()
    {
        double baseRollEnergy = 0;
        foreach (var modifier in GetAllModifiers<IRollEnergyModifier>())
        {
            baseRollEnergy = modifier.ModifyRollEnergy(baseRollEnergy);
        }
        return baseRollEnergy;
    }

    public void CheckRunNextWave()
    {
        if (CurrentTurn >= LevelConfig.TurnCount)
        {
            // end game
        }
        else
        {
            var turnConfig = LevelConfig.GetTurnConfig(CurrentTurn);
            GamePlay.StartNewWave(turnConfig);
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
        //if (IsRunning) UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        IncreaseEnergy(GameConfig.BaseEnergyPerSec * Time.deltaTime);
    }

    public void IncreaseEnergy(double value)
    {
        var newValue = NumberUtils.Round(State.energy + value, 2);
        State.energy = System.Math.Clamp(State.energy + value, 0, GameConfig.MaxEnergy);
    }

    #region Game Actions

    public void CardActionDone() => RollCards(true);
    public void CancelPlayingCard()
    {
        // tam thoi, sau nay se lam lai han hoi, phai action card thi moi tru cost + add vao card da choi
        PlayingCard = CardEnum.None;
        State.selectedCards.RemoveAt(State.selectedCards.Count - 1);
        InputStateEnum = InputStateEnum.SelectingCard;
    }
    public void CastLightnings(int times, Damage damage)
    {
        GamePlay?.CastGameLightnings(times, damage);
    }
    public void DoFrozenAllEnemies(float duration)
    {
        GamePlay?.FreezeEnemies(duration);
    }
    
    public void ReverseEnemies(Vector3 wPos)
    {
        var config = (TimeReverseCardConfig)Configs.GetCardConfig(CardEnum.TimeReverse);
        GamePlay.ReverseEnemies(wPos, config.Radius, config.ReverseTime);
    }

    public void DropBomb(Vector3 position)
    {
        if (InputStateEnum != InputStateEnum.PlayCard) return;
        // hardcode goi config, sau nay lay dmg va radius tu modifier
        var config = ((BombCardConfig)Configs.GetCardConfig(CardEnum.Bomb));
        GamePlay.DropBomb(position, config.Damage, config.Radius);
    }

    public void PlaceTower(int placeIndex, TowerEnum tower)
    {
        Debug.Log($"Place Tower {tower}");
        GamePlay?.PlaceTower(placeIndex, tower);
    }

    public void DropNapalm(Vector3 position)
    {
        if (InputStateEnum != InputStateEnum.PlayCard) return;
        // hardcode goi config, sau nay config cua cac the chuc nang se o cho rieng, the chi co config co ban
        var config = ((NapalmCardConfig)Configs.GetCardConfig(CardEnum.Napalm));
        GamePlay.DropNapalm(position, config.FireNumber, config.Radius, config.InstantlyDamage, config.DamageInterval, config.DamagePerSec, config.EachRadius);
        
    }

    #endregion
}
