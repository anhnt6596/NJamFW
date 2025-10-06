using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] CanvasGroup background;
    [SerializeField] Image health;

    public Unit Target { get; private set; }
    public void Setup(Unit target)
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
