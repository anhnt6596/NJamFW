using Core;
using Lean.Pool;
using System;
using UnityEngine;

public class EffectManager : MonoBehaviour, IManager
{
    public void Init()
    {

    }

    public void StartUp()
    {

    }

    public void Cleanup()
    {

    }

    public void SpawnLightning(Vector3 wPos)
    {
        var pos = wPos + Vector3.up * 2.2f;
        pos = GamePlayUtils.Y2Z(pos, -0.2f);
        var lightning = LeanPool.Spawn(ResourceProvider.Effect.lazerBlue, pos, Quaternion.identity, transform);
        lightning.Play();
        lightning.transform.localScale = Vector3.one * 0.5f;
        this.DelayCall(5, () => LeanPool.Despawn(lightning));
    }

    public void SpawnBombEffect(Vector3 wPos)
    {
        var pos = GamePlayUtils.Y2Z(wPos, -0.2f);
        var explosion = LeanPool.Spawn(ResourceProvider.Effect.bombExplosion, pos, Quaternion.identity, transform);
        explosion.Play();
        explosion.transform.localScale = Vector3.one * 0.4f;
        this.DelayCall(5, () => LeanPool.Despawn(explosion));
    }
}
