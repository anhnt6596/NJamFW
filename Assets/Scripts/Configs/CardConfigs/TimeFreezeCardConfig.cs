using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeFreeze", menuName = "Config/Card/TimeFreeze")]
public class TimeFreezeCardConfig : CardConfig
{
    [SerializeField] float freezeTime = 5;
    [SerializeField] int appearAtTotalRoll = 4;
    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        // all enemy freeze for freezeTime seconds
        game.DoFrozenAllEnemies(freezeTime);
        return InputStateEnum.SelectingCard;
    }
    public override bool CanBeRoll(Game game)
    {
        if (game.State.totalRolled < appearAtTotalRoll) return false;
        return base.CanBeRoll(game);
    }
    public override string GetDetailInfo(Game game)
    {
        return $"{freezeTime}s";
    }
}
