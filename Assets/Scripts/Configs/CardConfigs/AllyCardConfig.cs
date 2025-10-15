using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AnAlly", menuName = "Config/Card/Ally")]
public class AllyCardConfig : CardConfig, ICardPlayingRoad
{
    [SerializeField] AllyEnum ally;

    public AllyEnum Ally => ally;

    public Vector3 WPos { get; set; }

    public override void ApplyCardEffect(Game game)
    {
        game.GamePlay.SpawnAlly(Ally, WPos);
    }

    public override string GetPlayDescription(Game game)
    {
        var cardInfo = Configs.GetCardInfo(Card);
        return cardInfo.PlayDescription.Replace("@name#", cardInfo.DisplayName);
    }
}
