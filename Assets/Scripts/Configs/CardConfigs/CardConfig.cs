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
    [SerializeField] bool isUse = true;

    public CardEnum Card => card;
    public virtual int GetCost(Game game)
    {
        var useTime = game.State.selectedCards.Count(c => c == card);
        var nextCost = Mathf.FloorToInt(cost + useTime * escalatingCost);
        return Mathf.Clamp(nextCost, 0, (int)Configs.GamePlay.MaxEnergy);
    }
    public bool IsUse => isUse;  // bien tam thoi, neu false thi la config khong duoc su dung

    public virtual InputStateEnum ApplySellectedEffect(Game game)
    {
        return InputStateEnum.None;
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
