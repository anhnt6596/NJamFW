using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Lean.Touch.LeanSelectableByFinger;

public abstract class CardConfig : ScriptableObject
{
    [SerializeField] CardEnum card;
    [SerializeField] int cost = 1;
    [SerializeField] float escalatingCost = 0;
    [SerializeField] int maxSellectedTime = -1;
    [SerializeField] bool isAbilityCard = false;
    [SerializeField] bool isUse = true;

    public CardEnum Card => card;
    public bool IsAbilityCard => isAbilityCard; // neu la true, se la card real tume, neu false la card turn setup
    public bool IsUse => isUse;  // bien tam thoi, neu false thi la config khong duoc su dung
    public int BaseCost => cost;
    public float EscalatingCost => escalatingCost;
    public int MaxUsed => maxSellectedTime;
    public virtual int GetCost(Game game)
    {
        var useTime = game.State.selectedCards.Count(c => c == card);
        var nextCost = Mathf.FloorToInt(cost + useTime * escalatingCost);
        return Mathf.Clamp(nextCost, 0, (int)Configs.GamePlay.MaxEnergy);
    }

    public virtual InputStateEnum ApplySellectedEffect(Game game)
    {
        return InputStateEnum.SelectingCard;
    }
    public virtual string GetDetailInfo(Game game) => "";
    public virtual string GetPlayDescription(Game game) => "";
    public virtual bool CanBeRoll(Game game)
    {
        if (ReachLimitedRoll(game)) return false;
        return true;
    }

    protected bool ReachLimitedRoll(Game game)
    {
        return (maxSellectedTime != -1 && game.State.selectedCards.Count(c => c == card) >= maxSellectedTime);
    }
}
