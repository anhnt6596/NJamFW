using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "An Object Card", menuName = "Config/Card/Object")]
public class ObjectCardConfig : CardConfig, ICardPlayingTilePlace
{
    [SerializeField] ObjectEnum objectType;

    public ObjectEnum ObjectType => objectType;
    public GridPos GridPosition { get; set; }

    public override void ApplyCardEffect(Game game)
    {
        var gamePlay = game.GameField;
        gamePlay?.PlaceObject(ObjectType, GridPosition);
    }

    public override bool CanBeRoll(Game game)
    {
        return base.CanBeRoll(game);
    }

    public override string GetPlayDescription(Game game)
    {
        return "place";
        //var cardInfo = Configs.GetCardInfo(Card);
        //return cardInfo.PlayDescription.Replace("@name#", cardInfo.DisplayName);
    }
}
