using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Lightning", menuName = "Config/Card/Lightning")]
public class LightningCardConfig : CardConfig
{
    [SerializeField] int init = 1;
    [SerializeField] int increase = 2;
    [SerializeField] float damageEach = 100;
    [SerializeField] DamageEnum damageType = DamageEnum.Magic;

    public override void ApplySellectedEffect(Game game)
    {
        var times = GetLightingTime(game);
        // Do Lightning Random Enemies "times" times
    }

    public override string GetDetailInfo(Game game)
    {
        return $"x{GetLightingTime(game)}";
    }

    private int GetLightingTime(Game game)
    {
        var used = game.State.selectedCards.Count(c => c == CardEnum.Lightning);
        return init + increase * used;
    }
}
