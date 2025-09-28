using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] Slider sliderEnergy;
    Game game;

    private void OnEnable()
    {
        game = App.Get<GameManager>().RunningGame;
    }

    private void Update()
    {
        energyText.text = $"{Mathf.Floor(game.State.energy)}";
        sliderEnergy.value = game.State.energy / Configs.GamePlay.MaxEnergy;
    }
}
