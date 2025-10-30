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
        var gamePlay = game.GameField;
        gamePlay?.PlaceTower(PlacementIndex, tower);
    }

    public override bool CanBeRoll(Game game)
    {
        return base.CanBeRoll(game);
    }

    public override string GetPlayDescription(Game game)
    {
        var cardInfo = Configs.GetCardInfo(Card);
        return cardInfo.PlayDescription.Replace("@name#", cardInfo.DisplayName);
    }
}
