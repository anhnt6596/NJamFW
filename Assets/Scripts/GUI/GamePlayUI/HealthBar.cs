using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] CanvasGroup background;
    [SerializeField] Image health;

    public EnemyVisual Target { get; private set; }
    public void Setup(EnemyVisual target)
    {
        Target = target;
        Update();
    }

    private void Update()
    {
        if (Target != null)
        {
            health.fillAmount = Target.HP / Target.maxHP;
            var screenPos = Camera.main.WorldToScreenPoint(Target.healthNode.position);
            transform.position = screenPos;
        }
    }
}
