using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LightningPower", menuName = "Config/Card/Lightning Power")]
public class LightningPowerConfig : CardConfig, ILightningPowerModifier
{
    [SerializeField] float damageIncrease = 25;

    public float DamageIncrease => damageIncrease;

    public override void ApplySellectedEffect(Game game)
    {
        // do nothing
    }

    public override bool CanBeRoll(Game game)
    {
        if (!game.State.selectedCards.Contains(CardEnum.Lightning)) return false;
        return base.CanBeRoll(game);
    }

    public override string GetDetailInfo(Game game)
    {
        return $"+Power";
    }

    public float ModifyLightningDamageEnergy(float baseDmg)
    {
        return baseDmg + damageIncrease;
    }
}
