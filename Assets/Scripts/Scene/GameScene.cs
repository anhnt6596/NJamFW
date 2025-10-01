using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    private void OnEnable()
    {
        var game = App.Get<GameManager>().RunningGame;
        var level = ResourceProvider.GetLevel(game.Level);
        Instantiate(level, transform);
        this.DelayCall(1, () => game.StartGame());
    }
}
