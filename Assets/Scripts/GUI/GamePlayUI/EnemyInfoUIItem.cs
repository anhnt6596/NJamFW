using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoUIItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    public void Display(EnemyCount enemyCount)
    {
        image.sprite = ResourceProvider.GetEnemyIcon(enemyCount.type);
        text.text = $"x{enemyCount.count}";
    }
}