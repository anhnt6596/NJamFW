using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathHint : MonoBehaviour
{
    [SerializeField] List<GameObject> hintPaths;

    private void Awake()
    {
        hintPaths.ForEach(go => go.SetActive(false));
    }

    private void OnEnable()
    {
        Game.OnPhaseChanged += OnPhaseChanged;
    }

    private void OnDisable()
    {
        Game.OnPhaseChanged -= OnPhaseChanged;
    }

    private void OnPhaseChanged(int turn, TurnPhaseEnum phase)
    {
        if (phase != TurnPhaseEnum.Prepare)
        {
            hintPaths.ForEach(go => go.SetActive(false));
            return;
        }

        List<int> enablePaths = new();

        var game = App.Get<GameManager>().RunningGame;
        if (game == null) return;
        var level = game.Level;

        var levelConfig = Configs.GetLevelConfig(level);
        var turnConfig = levelConfig.GetTurnConfig(turn);
        for (int i = 0; i < turnConfig.EnemySpawnGroups.Count; i++)
        {
            var group = turnConfig.EnemySpawnGroups[i];
            if (!enablePaths.Contains(group.GateIdx)) enablePaths.Add(group.GateIdx);
        }

        for (int j = 0; j < hintPaths.Count; j++)
        {
            hintPaths[j].SetActive(enablePaths.Contains(j));
        }
    }
}
