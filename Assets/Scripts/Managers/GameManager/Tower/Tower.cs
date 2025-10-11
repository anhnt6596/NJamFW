using Core;
using DG.Tweening;
using Lean.Pool;
using System;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image fireIndicator;
    [SerializeField] Transform sniper;
    [SerializeField] Animator animator;
    public int Level { get; private set; } = 1;
    TowerConfig config;
    public Damage damage => config.GetAttackByLevel(Level);
    public TowerEnum TowerType { get; private set; }
    private float fireCooldown;
    private BaseBullet bulletPrefab;

    private EnemyVisual currentTarget;
    IGamePlay gamePlay;

    public void Setup(TowerEnum type, IGamePlay gamePlay)
    {
        TowerType = type;
        this.gamePlay = gamePlay;
        config = Configs.GetTowerConfig(TowerType);
        bulletPrefab = ResourceProvider.GetBullet(TowerType);
        Level = 1;
        DisplayLevel();
        fireCooldown = 1f / config.FireRate;
    }

    public void LevelUp()
    {
        Level++;
        DisplayLevel();
    }

    private void DisplayLevel() => levelText.text = $"Level {Level}";

    void Update()
    {
        if (fireCooldown > 0) fireCooldown -= Time.deltaTime;
        fireIndicator.fillAmount = 1 - fireCooldown * config.FireRate;

        if (fireCooldown <= 0f)
        {
            FindTarget();
            if (currentTarget != null)
            {
                Shoot(currentTarget);
                fireCooldown = 1f / config.FireRate;
            }
        }
    }

    private void FindTarget()
    {
        if (currentTarget != null && !currentTarget.isDead)
        {
            // if last target in range, do not change
            if (GamePlayUtils.IsInRange(currentTarget.transform.position, transform.position, config.Range)) return;
        }

        var enemies = gamePlay.Enemies;
        currentTarget = null;
        float nearestDest = Mathf.Infinity;

        foreach (var e in enemies)
        {
            Vector2 diff = e.transform.position - transform.position;
            var remainDest = e.remainingDist;
            var inRange = GamePlayUtils.IsInRange(e.transform.position, transform.position, config.Range);

            if (inRange && remainDest < nearestDest)
            {
                nearestDest = remainDest;
                currentTarget = e;
            }
        }
    }

    void Shoot(EnemyVisual target)
    {
        if (sniper != null)
        {
            var a = GamePlayUtils.GetDirection2Index(target.transform.position - sniper.transform.position);
            sniper.localScale = new Vector3(a == 1 ? 1 : -1, 1, 1) * 1.2f;
        }

        if (bulletPrefab == null || firePoint == null) return;

        if (animator) animator.SetTrigger("Shoot");

        SoundManager.Play(ResourceProvider.Sound.combat.tower.GetShotSound(config.Type));

        BaseBullet bullet = LeanPool.Spawn(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.SetDamage(damage);
        bullet.Display();
        if (bullet != null)
        {
            bullet.SetTarget(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (config == null) return;
        Gizmos.color = Color.yellow;

        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(config.Range.x * 2, config.Range.y * 2, 1));
        Gizmos.matrix = matrix;
        Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        Gizmos.matrix = Matrix4x4.identity;
    }

}