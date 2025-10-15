using System.Linq;
using UnityEditor;
using UnityEngine;

// khong lam luon vi buff theo thoi gian can status cho tower
[CreateAssetMenu(fileName = "Tower Buff", menuName = "Config/Card/Tower Buff")]
public class TowerBuffCardConfig : CardConfig
{
    [SerializeField] float mult = 2;

    public override bool CanBeRoll(Game game)
    {
        if (!HaveAnyTower(game)) return false;
        return base.CanBeRoll(game);
    }

    private bool HaveAnyTower(Game game)
    {
        var selected = game.State.selectedCards;
        if (selected.Contains(CardEnum.ArcherTower)) return true;
        if (selected.Contains(CardEnum.MageTower)) return true;
        if (selected.Contains(CardEnum.ArtilleryTower)) return true;
        return false;
    }

    public override string GetDetailInfo(Game game)
    {
        return $"Tower x2 dmg";
    }
}
