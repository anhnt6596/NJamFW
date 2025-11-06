using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static SoundResourceSet;

[CreateAssetMenu(fileName = "OnTowerCardConfig", menuName = "Config/Card/OnTower")]
public class OnTowerCardConfig : CardConfig, ICardPlayingOnTower
{
    [SerializeField] OnTowerEnum type;
    public OnTowerEnum Type => type;
    public Tower Tower { get; set; } // sau doi sang interface de khong phu thuoc vao Tower class

    public override void ApplyCardEffect(Game game)
    {
        var gamePlay = game.GameField;
        gamePlay?.PlaceOnTower(Type, Tower);
    }

    public override bool CanBeRoll(Game game)
    {
        var gamePlay = game.GameField;
        var towers = gamePlay.Towers;
        var remainingSlot = towers.Count(t => t.Ally != null);
        // neu het cho dat thap ma k co thap nao cung loai thi khong the roll ra
        if (remainingSlot <= 0) return false;
        return base.CanBeRoll(game);
    }

    public override string GetPlayDescription(Game game)
    {
        return "Place on a tower";
    }
}
