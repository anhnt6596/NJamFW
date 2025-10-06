using System.Collections.Generic;
using UnityEngine;

public interface IGamePlay
{
    List<EnemyVisual> Enemies { get; }
    void StartNewWave(WaveConfig waveConfig);

    void CastGameLightnings(int times, Damage damage);
    void FreezeEnemies(float duration);
    void ReverseEnemies(Vector3 wPos, Vector3 radius, float duration);
    void DropBomb(Vector3 position, Damage damage, Vector2 radius);
    bool CheckPlaceTowerPosition(Vector3 wPos, TowerEnum tower, out int placeIndex);
    void PlaceTower(int placeIndex, TowerEnum tower);
    bool IsWPosInPolygon(Vector3 wPos);
    void SpawnAlly(AllyEnum allyType, Vector3 wPos);
    void DropNapalm(Vector3 position, int fireNumber, Vector2 radius, Damage instantlyDamage, float interval, float damagePerSec, Vector2 eachRadius);
}