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
    public Vector2 rangeCheck = new Vector2(1.5f, 1.05f);
    public Vector2 range = new Vector2(2, 1.4f);

    IGamePlay gamePlay;
    bool isExploded = false;
    public void Setup(IGamePlay gamePlay)
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
        if (gamePlay.Enemies.Count == 0) return;

        timeCounter -= Time.deltaTime;
        if (timeCounter < 0)
        {
            timeCounter += intervalCheck;
            CheckExplode();
        }
    }

    private void CheckExplode()
    {
        foreach (var enemy in gamePlay.Enemies)
        {
            if (GamePlayUtils.IsInRange(enemy.transform.position, transform.position, rangeCheck))
            {
                Explode();
                break;
            }
        }
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
            App.Get<EffectManager>().SpawnBombEffect(transform.position, 0.65f);
            DealDamage();
            LeanPool.Despawn(this);
        });
    }

    private void DealDamage()
    {
        for (int i = gamePlay.Enemies.Count; i > 0 ; i--)
        {
            var enemy = gamePlay.Enemies[i - 1];
            var v = GamePlayUtils.CheckElipse(enemy.transform.position, transform.position, range);
            if (v < 1) enemy.TakeDamage(damage * GamePlayUtils.GetAoEDamageMultiplier(v, 0.45f));
        }
    }
}
