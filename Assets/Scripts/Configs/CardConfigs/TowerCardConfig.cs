using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerCardConfig", menuName = "Config/Card/Tower")]
public class TowerCardConfig : CardConfig, ICardPlayingTowerPlace
{
    [SerializeField] TowerEnum tower;
    public TowerEnum Tower => tower;

    public int PlacementIndex { get; set; }
    public override void ApplyCardEffect(Game game)
    {
        var gamePlay = game.GamePlay;
        gamePlay?.PlaceTower(PlacementIndex, tower);
    }

    public override bool CanBeRoll(Game game)
    {
        var gamePlay = game.GamePlay;
        var towers = gamePlay.Towers;
        // neu het cho dat thap ma k co thap nao cung loai thi khong the roll ra
        bool isFull = towers.Count >= gamePlay.TowerPlacementCount;
        if (isFull && towers.Count(t => t.TowerType == Tower) <= 0) return false;
        return base.CanBeRoll(game);
    }

    public override string GetPlayDescription(Game game)
    {
        var cardInfo = Configs.GetCardInfo(Card);
        return cardInfo.PlayDescription.Replace("@name#", cardInfo.DisplayName);
    }
}
