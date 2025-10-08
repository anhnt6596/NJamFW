using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Lightning", menuName = "Config/Card/Lightning")]
public class LightningCardConfig : CardConfig
{
    [SerializeField] int init = 1;
    [SerializeField] int increase = 1;
    [SerializeField] int max = 10;
    [SerializeField] float damageEach = 100;
    [SerializeField] int maxEnergy = 3;
    [SerializeField] DamageEnum damageType = DamageEnum.Magic;

    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        var times = GetLightingTime(game);

        float dmg = damageEach;
        foreach (var modifier in game.GetAllModifiers<ILightningPowerModifier>())
        {
            dmg = modifier.ModifyLightningDamageEnergy(dmg);
        }

        game.CastLightnings(times, new Damage(dmg, damageType));

        return InputStateEnum.SelectingCard;
    }

    public override string GetDetailInfo(Game game)
    {
        return $"x{GetLightingTime(game)}";
    }

    private int GetLightingTime(Game game)
    {
        var used = game.State.selectedCards.Count(c => c == CardEnum.Lightning);
        return Mathf.Min(init + increase * used, max);
    }

    public override int GetCost(Game game)
    {
        return Mathf.Min(base.GetCost(game), maxEnergy); 
    }
}
