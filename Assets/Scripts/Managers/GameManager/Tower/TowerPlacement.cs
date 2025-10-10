using Lean.Pool;
using System;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] Vector2 placeRadius = new Vector2(1, 0.7f);
    [SerializeField] Vector3 focusPos, focusPosHaveTower;
    public Tower Tower { get; private set; }

    public bool CheckPostion(Vector3 wPos, TowerEnum tower)
    {
        if (Tower != null && Tower.TowerType != tower) return false;
        // add them ca neu bam trung tower cu da xay sau
        return GamePlayUtils.IsInRange(wPos, transform.position, placeRadius);
    }

    public void BuildTower(TowerEnum tower, IGamePlay gamePlay)
    {
        if (!Tower)
        {
            var towerPrefab = ResourceProvider.GetTower(tower);
            Tower = Instantiate(towerPrefab, transform);
            Tower.Setup(tower, gamePlay);
            Tower.transform.localPosition = Vector3.back * 0.001f;
            SoundManager.Play(ResourceProvider.Sound.combat.tower.build);
        }
        else
        {
            Tower.LevelUp();
            App.Get<EffectManager>().SpawnUpgradeEff(transform.position + Vector3.up * 0.5f);
            SoundManager.Play(ResourceProvider.Sound.combat.tower.upgrade);
        }
    }

    ArrowFocus focusObject;

    public void Focus(bool isShow)
    {
        if (focusObject == isShow) return;
        if (isShow)
        {
            focusObject = LeanPool.Spawn(ResourceProvider.Effect.arrowFocus, transform);
            focusObject.transform.localPosition = Tower ? focusPosHaveTower : focusPos;
        }
        else
        {
            LeanPool.Despawn(focusObject);
            focusObject = null;
        }
    }
}