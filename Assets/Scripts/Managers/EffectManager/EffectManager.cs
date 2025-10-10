using Core;
using DG.Tweening;
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
        this.DelayCall(5, () => LeanPool.Despawn(lightning));
        lightning.transform.localScale = Vector3.one * 0.5f;

        SoundManager.Play(ResourceProvider.Sound.combat.lightning);
    }

    public void SpawnBombEffect(Vector3 wPos, float scale = 0.4f)
    {
        var pos = GamePlayUtils.Y2Z(wPos, -0.2f);
        var explosion = LeanPool.Spawn(ResourceProvider.Effect.bombExplosion, pos, Quaternion.identity, transform);
        explosion.transform.localScale = Vector3.one * scale;
        this.DelayCall(5, () => LeanPool.Despawn(explosion));

        SoundManager.Play(ResourceProvider.Sound.combat.explode);
    }

    public void SpawnSmokeEffect(Vector3 wPos)
    {
        var pos = GamePlayUtils.Y2Z(wPos, -0.2f);
        var smoke = LeanPool.Spawn(ResourceProvider.Effect.smoke, pos, Quaternion.identity, transform);
        smoke.transform.localScale = Vector3.one * 0.15f;
        this.DelayCall(5, () => LeanPool.Despawn(smoke));
    }

    public float NapalmDrop(Vector3 position)
    {
        var pos = GamePlayUtils.Y2Z(position, -0.2f);
        for (int i = 0; i < 10; i++)
        {
            var fire = LeanPool.Spawn(ResourceProvider.Effect.burning, pos, Quaternion.identity, transform);
            fire.transform.localScale = Vector3.one * 0.3f;
            fire.transform.position = GamePlayUtils.Y2Z(pos + Vector3.up * 12, -0.2f);
            fire.transform.DOMove(pos, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                App.Get<EffectManager>().SpawnBombEffect(position, 0.3f);
                LeanPool.Despawn(fire);
            });
        }
        return 0.5f;
    }

    public void SpellCastEff(Vector3 startPos, Vector3 endPos, float dur, bool isJump = false, System.Action cb = null)
    {
        var spell = LeanPool.Spawn(ResourceProvider.Effect.spellCast);
        spell.transform.position = startPos;
        var seq = DOTween.Sequence();
        if (!isJump) seq.Append(spell.transform.DOMove(endPos, dur).SetEase(Ease.OutSine));
        else seq.Append(spell.transform.DOJump(endPos, 2, 1, dur).SetEase(Ease.OutSine));
        seq.AppendCallback(() =>
        {
            cb?.Invoke();
            spell.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        });
        seq.AppendInterval(2);
        seq.AppendCallback(() => LeanPool.Despawn(spell));
    }

    public void SpawnUpgradeEff(Vector3 wPos, float scale = 0.3f)
    {
        var pos = GamePlayUtils.Y2Z(wPos, -0.2f);
        var upgrade = LeanPool.Spawn(ResourceProvider.Effect.upgrade, pos, Quaternion.identity, transform);
        upgrade.transform.localScale = Vector3.one * scale;
        this.DelayCall(5, () => LeanPool.Despawn(upgrade));
    }
}
