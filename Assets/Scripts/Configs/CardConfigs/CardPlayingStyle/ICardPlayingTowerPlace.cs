public interface ICardPlayingTowerPlace : ICardPlayingStyle
{
    TowerEnum Tower { get; }
    int PlacementIndex { get; set; }
}