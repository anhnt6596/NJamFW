using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] CanvasGroup background;
    [SerializeField] Image health;

    public Unit Target { get; private set; }
    public void Setup(Unit target, Color color)
    {
        Target = target;
        health.color = color;
        LateUpdate();
    }

    private void LateUpdate()
    {
        if (Target != null)
        {
            float hpRatio = Target.HP / Target.maxHP;
            health.fillAmount = Mathf.Lerp(health.fillAmount, hpRatio, 0.1f);
            //var screenPos = Camera.main.WorldToScreenPoint(Target.healthNode.position);
            transform.position = Target.healthNode.position;
            if (hpRatio == 1) background.alpha = 0;
            else background.alpha = Mathf.Lerp(background.alpha, 1, 0.1f);
        }
    }
}
