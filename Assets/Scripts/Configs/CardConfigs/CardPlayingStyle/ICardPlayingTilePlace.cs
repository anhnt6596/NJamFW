public interface ICardPlayingTilePlace : ICardPlayingStyle
{
    PlaceObjectEnum ObjectType { get; }
    GridPos GridPosition { get; set; }
}