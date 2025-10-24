public interface ICardPlayingTilePlace : ICardPlayingStyle
{
    ObjectEnum ObjectType { get; }
    GridPos GridPosition { get; set; }
}