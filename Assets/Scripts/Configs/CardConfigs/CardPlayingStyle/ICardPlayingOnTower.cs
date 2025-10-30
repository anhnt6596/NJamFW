public interface ICardPlayingOnTower : ICardPlayingStyle
{
    OnTowerEnum Type { get; }
    Tower Tower { get; set; }
}