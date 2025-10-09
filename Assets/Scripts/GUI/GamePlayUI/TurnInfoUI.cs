using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private TextMeshProUGUI turnText;

    private void Awake()
    {
        display.SetActive(false);
    }

    private void OnEnable()
    {
        Game.OnPhaseChanged += OnPhaseChanged;
    }

    private void OnDisable()
    {
        Game.OnPhaseChanged -= OnPhaseChanged;
    }

    private void OnPhaseChanged(int turnIdx, TurnPhaseEnum phase)
    {
        if (phase != TurnPhaseEnum.Prepare)
        {
            display.SetActive(false);
            return;
        }
        else
        {
            display.SetActive(true);
            turnText.text = $"Turn {turnIdx + 1}";
        }
    }

    public void OnClickStart()
    {
        App.Get<GameManager>().RunningGame.ReadyForTurn();
    }
}
