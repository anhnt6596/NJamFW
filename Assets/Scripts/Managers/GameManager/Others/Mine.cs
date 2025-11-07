using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mine : MonoBehaviour
{
    [SerializeField] SpriteRenderer eye;
    // setting truc tiep thong so, dua vao setup sau
    public Damage damage = new Damage(300, DamageEnum.Magic);
    public float rangeCheck = 2;
    public float range = 3;

    IGameField gamePlay;
    bool isExploded = false;
    public void Setup(IGameField gamePlay)
    {
        this.gamePlay = gamePlay;
        isExploded = false;
        eye.SetAlpha(0);
    }

    private float intervalCheck = 0.2f;
    private float timeCounter = 0f;

    private void Update()
    {
        if (gamePlay == null || isExploded) return;

        timeCounter -= Time.deltaTime;
        if (timeCounter < 0)
        {
            timeCounter += intervalCheck;
            CheckExplode();
        }
    }

    private void CheckExplode()
    {
        var enemies = new List<Enemy>(); // sau move ra thanh 1 field
        gamePlay.EnemySpatialHash.Query(transform.position, rangeCheck, enemies);

        if (enemies.Count > 0) Explode();
    }

    Sequence seq;

    private void Explode()
    {
        isExploded = true;
        seq?.Kill();
        seq = DOTween.Sequence();
        seq.Append(eye.DOFade(1, 0.2f));
        seq.Append(eye.DOFade(0, 0.2f));
        seq.Append(eye.DOFade(1, 0.2f));
        seq.Append(eye.DOFade(0, 0.15f));
        seq.Append(eye.DOFade(1, 0.15f));
        seq.Append(eye.DOFade(0, 0.1f));
        seq.Append(eye.DOFade(1, 0.1f));
        seq.Append(eye.DOFade(0, 0.05f));
        seq.Append(eye.DOFade(1, 0.05f));
        seq.AppendCallback(() =>
        {
            App.Get<EffectManager>().SpawnExplodeEffect(transform.position, 0.65f);
            CameraShake.Shake(0.3f, 0.1f);
            DealDamage();
            LeanPool.Despawn(this);
        });
    }

    private void DealDamage()
    {
        var enemies = new List<Enemy>(); // sau move ra thanh 1 field
        gamePlay.EnemySpatialHash.Query(transform.position, range, enemies);
        enemies.ForEach(e => e.TakeDamage(damage));
        // sau them tinh dmg aoe, ngoai ria nhan it dmg hon
    }
}
