using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Mine", menuName = "Config/Card/Mine")]
public class MineCardConfig : CardConfig, ICardPlayingRoad
{
    [SerializeField] int maxEnergy = 1;

    public Vector3 WPos { get; set; }

    public override int GetCost(Game game)
    {
        return Mathf.Min(base.GetCost(game), maxEnergy);
    }

    public override void ApplyCardEffect(Game game)
    {
        game.GamePlay.DropMine(WPos);
    }

    public override string GetPlayDescription(Game game) => Configs.GetCardInfo(Card).PlayDescription;
}
