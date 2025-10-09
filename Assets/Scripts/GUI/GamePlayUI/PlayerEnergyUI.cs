using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] Slider sliderEnergy;
    [SerializeField] List<Image> sliderItems;
    Game game;

    private void OnEnable()
    {
        game = App.Get<GameManager>().RunningGame;
    }

    private void Update()
    {
        var energy = (float)game.State.energy;
        energyText.text = $"{Mathf.Floor(energy)}";
        sliderEnergy.value = energy / (float)Configs.GamePlay.MaxEnergy;
        for (int i = 0; i < sliderItems.Count; i++)
        {
            var item = sliderItems[i];
            var energyFloor = Mathf.FloorToInt(energy);
            if (i < energyFloor)
            {
                if (item.color.a < 0.9f)
                {
                    var seq = DOTween.Sequence();
                    seq.Append(item.transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutQuart));
                    seq.Append(item.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
                }
                item.SetAlpha(1);
                item.fillAmount = 1;
            }
            else
            {
                item.SetAlpha(0.3f);
                if (i == energyFloor)
                {
                    item.fillAmount = (float)game.State.energy - energyFloor;
                }
                else item.fillAmount = 0;
            }
        }
    }
}
