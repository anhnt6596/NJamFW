using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerCardConfig", menuName = "Config/Card/Tower")]
public class TowerCardConfig : CardConfig
{
    [SerializeField] TowerEnum tower;
    public TowerEnum Tower => tower;
    
    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        return InputStateEnum.PlayCard;
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

    public override string GetPlayDescription(Game game) => $"Tab to build {Tower}";
}
