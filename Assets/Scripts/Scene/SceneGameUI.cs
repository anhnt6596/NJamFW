using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnText;
    private void OnEnable()
    {
        Game.OnPhaseChanged += OnPhaseChanged;
        turnText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Game.OnPhaseChanged += OnPhaseChanged;
    }

    private void OnPhaseChanged(int turn, TurnPhaseEnum phase)
    {
        if (turn < 0 || phase != TurnPhaseEnum.Combat)
        {
            turnText.gameObject.SetActive(false);
            return;
        }

        turnText.gameObject.SetActive(true);
        turnText.text = $"Turn {turn + 1}";
    }

    public void OnClickPause()
    {
        App.Get<GameManager>().PauseGame();
    }

    private void DisplayTurnText()
    {
    }
}
