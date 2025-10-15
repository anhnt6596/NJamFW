using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeFreeze", menuName = "Config/Card/TimeFreeze")]
public class TimeFreezeCardConfig : CardConfig, ICardPlayingInstantly
{
    [SerializeField] float freezeTime = 5;
    [SerializeField] int appearAtTotalRoll = 4;

    public override bool CanBeRoll(Game game)
    {
        if (game.State.totalRolled < appearAtTotalRoll) return false;
        return base.CanBeRoll(game);
    }

    public override void ApplyCardEffect(Game game)
    {
        game.DoFrozenAllEnemies(freezeTime);
    }

    public override string GetDetailInfo(Game game)
    {
        return $"{freezeTime}s";
    }
}
