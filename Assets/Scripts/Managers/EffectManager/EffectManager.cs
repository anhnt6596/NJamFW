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

    public void SpawnLightning(Vector3 wPos, bool upgraded)
    {
        var pos = wPos + Vector3.up * 2.2f;
        pos = GamePlayUtils.Y2Z(pos, -0.2f);

        ParticleSystem effectLazer = upgraded ? ResourceProvider.Effect.lazerPurple : ResourceProvider.Effect.lazerBlue;

        var lightning = LeanPool.Spawn(effectLazer, pos, Quaternion.identity, transform);
        lightning.Play();
        lightning.transform.localScale = Vector3.one * 0.5f;
        this.DelayCall(5, () => LeanPool.Despawn(lightning));
    }

    public void SpawnBombEffect(Vector3 wPos)
    {
        var pos = GamePlayUtils.Y2Z(wPos, -0.2f);
        var explosion = LeanPool.Spawn(ResourceProvider.Effect.bombExplosion, pos, Quaternion.identity, transform);
        explosion.transform.localScale = Vector3.one * 0.4f;
        this.DelayCall(5, () => LeanPool.Despawn(explosion));
    }

    public void SpawnSmokeEffect(Vector3 wPos)
    {
        var pos = GamePlayUtils.Y2Z(wPos, -0.2f);
        var smoke = LeanPool.Spawn(ResourceProvider.Effect.smoke, pos, Quaternion.identity, transform);
        smoke.transform.localScale = Vector3.one * 0.15f;
        this.DelayCall(5, () => LeanPool.Despawn(smoke));
    }
}
