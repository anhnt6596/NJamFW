using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AnAlly", menuName = "Config/Card/Ally")]
public class AllyCardConfig : CardConfig
{
    [SerializeField] AllyEnum ally;

    public AllyEnum Ally => ally;

    public override InputStateEnum ApplySellectedEffect(Game game)
    {
        return InputStateEnum.PlayCard;
    }

    public override string GetPlayDescription(Game game)
    {
        return $"Place {ally} to the road";
    }
}
