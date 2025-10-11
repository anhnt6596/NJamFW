using DG.Tweening;
using Lean.Pool;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ArtilleryBullet : BaseBullet
{
    [SerializeField] Vector2 radius = new Vector2(1f, 0.7f);
    Vector3 targetPos;

    public override void SetTarget(EnemyVisual enemy)
    {
        targetPos = enemy.GetFuturePosition(1/speed);
        transform.DOJump(targetPos, 2.5f, 1, 1/speed)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                LeanPool.Despawn(this);
                App.Get<EffectManager>().SpawnExplodeEffect(transform.position, 0.25f);
                var enemies = App.Get<GameManager>().RunningGame.GamePlay.Enemies;
                for (int i = enemies.Count; i > 0; i--)
                {
                    var enemy = enemies[i - 1];
                    var v = GamePlayUtils.CheckElipse(enemy.transform.position, transform.position, radius);
                    if (v < 1) enemy.TakeDamage(damage * GamePlayUtils.GetAoEDamageMultiplier(v, 0.1f));
                }
            });
    }
}