using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    private void OnEnable()
    {
        var game = App.Get<GameManager>().RunningGame;
        var stage = ResourceProvider.GetStage(game.Level);
        Instantiate(stage, transform);
    }
}
