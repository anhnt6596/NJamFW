using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialize : MonoBehaviour
{
    [SerializeField] Transform sceneGameCanvas;
    [SerializeField] Transform sceneGameUI;
    private void Start()
    {
        var game = App.Get<GameManager>().CreateGame();

        // Instantiate Level
        var levelPrefab = ResourceProvider.GetLevel(game.Level);
        var level = Instantiate(levelPrefab, transform);
        level.Game = game;
        game.GamePlay = level;

        // Instantiate UI
        Instantiate(sceneGameUI, sceneGameCanvas);

        game.StartGame();
    }
}
