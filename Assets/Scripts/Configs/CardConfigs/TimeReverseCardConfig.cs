using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeReverse", menuName = "Config/Card/TimeReverse")]
public class TimeReverseCardConfig : CardConfig
{
    [SerializeField] float reverseTime = 4;
    [SerializeField] int appearAtTotalRoll = 6;
    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        // all enemy freeze for freezeTime seconds
        game.DoReverseAllEnemies(reverseTime);
        return InputStateEnum.None;
    }
    public override bool CanBeRoll(Game game)
    {
        if (game.State.totalRolled < appearAtTotalRoll) return false;
        return base.CanBeRoll(game);
    }
    public override string GetDetailInfo(Game game)
    {
        return $"{4}s";
    }
}
