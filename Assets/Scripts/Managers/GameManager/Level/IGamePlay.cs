using System.Collections.Generic;
using UnityEngine;

public interface IGamePlay
{
    List<EnemyVisual> Enemies { get; }
    void OnNewWaveStarted(WaveConfig waveConfig);

    void CastGameLightnings(int times, Damage damage);
    void OnAllEnemiesFroze(float duration);
    void OnAllEnemiesReversed(float duration);
    void OnBombDropped(Vector3 position, Damage damage, Vector2 radius);
    bool CheckPlaceTowerPosition(Vector3 wPos, TowerEnum tower, out int placeIndex);
    void PlaceTower(int placeIndex, TowerEnum tower);
}