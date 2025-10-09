using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Mine", menuName = "Config/Card/Mine")]
public class MineCardConfig : CardConfig
{
    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        return InputStateEnum.PlayCard;
    }

    public override string GetPlayDescription(Game game) => "Tap to drop Mine";
}
