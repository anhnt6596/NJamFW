using System.Collections.Generic;
using UnityEngine;

public class EnemyInfoUI : MonoBehaviour
{
    [SerializeField] private EnemyInfoUIItem enemyInfoUIItemPrefab;
    [SerializeField] private Transform parent;
    private List<EnemyInfoUIItem> enemyInfoUIItems = new List<EnemyInfoUIItem>();
    
    public void Display(List<EnemyCount> enemyInfo)
    {
        foreach (var enemyInfoUIItem in enemyInfoUIItems)
        {
            Destroy(enemyInfoUIItem.gameObject);
        }
        enemyInfoUIItems.Clear();
        foreach (var enemy in enemyInfo)
        {
            EnemyInfoUIItem item = Instantiate(enemyInfoUIItemPrefab, parent);
            item.Display(enemy);
            enemyInfoUIItems.Add(item);
        }
    }
}