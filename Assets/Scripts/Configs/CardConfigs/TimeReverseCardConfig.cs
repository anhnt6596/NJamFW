using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeReverse", menuName = "Config/Card/TimeReverse")]
public class TimeReverseCardConfig : CardConfig
{
    [SerializeField] float reverseTime = 4;
    [SerializeField] int appearAtTotalRoll = 6;
    [SerializeField] Vector2 radius = new Vector2(5, 3.5f);

    public Vector2 Radius => radius;
    public float ReverseTime => reverseTime;
    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        return InputStateEnum.PlayCard;
    }
    public override bool CanBeRoll(Game game)
    {
        if (game.State.totalRolled < appearAtTotalRoll) return false;
        return base.CanBeRoll(game);
    }
    public override string GetDetailInfo(Game game)
    {
        return $"{reverseTime}s";
    }

    public override string GetPlayDescription(Game game) => $"Tap to reverse time all enemy in an area {reverseTime}s";
}
