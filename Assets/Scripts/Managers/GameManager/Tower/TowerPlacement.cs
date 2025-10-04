using System;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] Vector2 placeRadius = new Vector2(1, 0.7f);
    public Tower Tower { get; private set; }
    private bool isHighlighed = false;
    public bool IsHighlighted => isHighlighed;

    public void SetHighlight(bool status)
    {
        isHighlighed = status;
        // enable or disable highlight effect
    }

    public bool CheckPostion(Vector3 wPos, TowerEnum tower)
    {
        if (Tower != null && Tower.TowerType != tower) return false;
        return GamePlayUtils.IsInRange(wPos, transform.position, placeRadius);
    }

    public void BuildTower(TowerEnum tower)
    {
        if (!Tower)
        {
            var towerPrefab = ResourceProvider.GetTower(tower);
            Tower = Instantiate(towerPrefab, transform);
            Tower.transform.localPosition = Vector3.back * 0.001f;
        }
        else
        {
            Tower.LevelUp();
        }
    }
}