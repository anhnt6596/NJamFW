using Core;
using Lean.Pool;
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
        var pos = wPos + Vector3.up * 2.2f + Vector3.back * 1f;
        var lightning = LeanPool.Spawn(ResourceProvider.Effect.lazerBlue, pos, Quaternion.identity, transform);
        lightning.Play();
        lightning.transform.localScale = Vector3.one * 0.5f;
        this.DelayCall(5, () => LeanPool.Despawn(lightning));
    }
}
