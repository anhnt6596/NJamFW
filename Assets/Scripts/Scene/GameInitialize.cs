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
        var gameMgr = App.Get<GameManager>();
        var game = gameMgr.CreateGame();

        // Instantiate Level
        var levelPrefab = ResourceProvider.GetLevel(game.Level);
        var level = Instantiate(levelPrefab, transform);
        level.Game = game;
        game.GamePlay = level;

        // Instantiate UI
        Instantiate(sceneGameUI, sceneGameCanvas);

        this.DelayCall(1, () => App.Get<GUIEffectManager>().BannerAnounce("3"));
        this.DelayCall(2, () => App.Get<GUIEffectManager>().BannerAnounce("2"));
        this.DelayCall(3, () => App.Get<GUIEffectManager>().BannerAnounce("1"));
        this.DelayCall(4, () => App.Get<GUIEffectManager>().BannerAnounce("Ready!"));
        this.DelayCall(5, () =>
        {
            gameMgr.CheckShowTutPopup(0);
            game.StartGame();
        });


        SoundManager.PlayMusic(ResourceProvider.Sound.music.combatMusic);
    }
}
