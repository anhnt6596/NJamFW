using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Lean.Touch.LeanSelectableByFinger;

public abstract class CardConfig : ScriptableObject
{
    [SerializeField] CardEnum card;
    [SerializeField] int cost = 1;
    [SerializeField] int escalatingCost = 0;
    [SerializeField] int maxSellectedTime = -1;
    [SerializeField] bool isUse = true;

    public CardEnum Card => card;
    public virtual int GetCost(Game game)
    {
        var useTime = game.State.selectedCards.Count(c => c == card);
        return Mathf.Clamp(cost + useTime * escalatingCost, 0, (int)Configs.GamePlay.MaxEnergy);
    }
    public bool IsUse => isUse;  // bien tam thoi, neu false thi la config khong duoc su dung

    public abstract void ApplySellectedEffect(Game game);
    public virtual string GetDetailInfo(Game game) => "";
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
