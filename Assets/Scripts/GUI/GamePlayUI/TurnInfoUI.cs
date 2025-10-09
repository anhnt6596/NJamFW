using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private EnemyInfoUI enemyInfoUI;

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
            DisplayEnemyInfo();
        }
    }

    private void DisplayEnemyInfo()
    {
        Game runningGame = App.Get<GameManager>().RunningGame;
        var config = Configs.GetLevelConfig(runningGame.Level).GetTurnConfig(runningGame.CurrentTurn);
        var enemyInfo = GamePlayUtils.GetEnemyInfoFromConfig(config);
        enemyInfoUI.Display(enemyInfo);
    }
    public void OnClickStart()
    {
        App.Get<GameManager>().RunningGame.ReadyForTurn();
    }
}
