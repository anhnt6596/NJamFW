using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Image healthSlider;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Transform castNode;

    Game game;
    private void OnEnable()
    {
        game = App.Get<GameManager>().RunningGame;
        Game.HealthChanged += OnHealthChanged;
        healthText.text = $"{game.State.baseHealth} / {Configs.GamePlay.BaseHealth}";
    }

    private void OnDisable()
    {
        Game.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int currentHealth, int lastHealth)
    {
        float healthPercent = (float)currentHealth / Configs.GamePlay.BaseHealth;
        healthText.text = $"{currentHealth} / {Configs.GamePlay.BaseHealth}";
        animator.SetTrigger("Damage");
    }

    private void Update()
    {
        if (game == null) return;
        var current = healthSlider.fillAmount;
        healthSlider.fillAmount = Mathf.Lerp(game.State.baseHealth / (float)Configs.GamePlay.BaseHealth, current, 0.1f);
    }

    float castTime = 0.4f;
    public void DoCastAnim(Action<Vector3> callback)
    {
        animator.SetTrigger("Cast");
        this.DelayCall(castTime, () => callback?.Invoke(castNode.position));
    }
}
