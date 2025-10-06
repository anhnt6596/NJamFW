using System.Collections.Generic;
using UnityEngine;

public interface IGamePlay
{
    List<EnemyVisual> Enemies { get; }
    void OnNewWaveStarted(WaveConfig waveConfig);

    void CastGameLightnings(int times, Damage damage);
    void OnAllEnemiesFroze(float duration);
    void OnAllEnemiesReversed(Vector3 wPos, Vector3 radius, float duration);
    void OnBombDropped(Vector3 position, Damage damage, Vector2 radius);
    bool CheckPlaceTowerPosition(Vector3 wPos, TowerEnum tower, out int placeIndex);
    void PlaceTower(int placeIndex, TowerEnum tower);
    bool IsWPosInPolygon(Vector3 wPos);
    void SpawnAlly(AllyEnum allyType, Vector3 wPos);
}