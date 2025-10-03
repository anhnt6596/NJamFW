using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialize : MonoBehaviour
{
    private void Start()
    {
        var game = App.Get<GameManager>().RunningGame;
        var level = ResourceProvider.GetLevel(game.Level);
        Instantiate(level, transform);
        game.StartGame();
    }
}
