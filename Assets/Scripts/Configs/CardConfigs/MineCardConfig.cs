using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Mine", menuName = "Config/Card/Mine")]
public class MineCardConfig : CardConfig
{
    [SerializeField] int maxEnergy = 1;

    public override int GetCost(Game game)
    {
        return Mathf.Min(base.GetCost(game), maxEnergy);
    }

    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        return InputStateEnum.PlayCard;
    }

    public override string GetPlayDescription(Game game) => "Tap to drop Mine";
}
