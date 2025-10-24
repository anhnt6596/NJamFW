public interface ICardPlayingTilePlace : ICardPlayingStyle
{
    ObjectEnum ObjectType { get; }
    (int, int) GridPosition { get; set; }
}