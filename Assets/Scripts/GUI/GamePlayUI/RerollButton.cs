using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RerollButton : MonoBehaviour
{
    [SerializeField] Button rerollButton;
    [SerializeField] Image rerollButtonEnergyLoad;
    [SerializeField] TextMeshProUGUI freeRollText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] GameObject costSlot;

    Game game;

    private void OnEnable()
    {
        game = App.Get<GameManager>().RunningGame;
        costText.text = Configs.GamePlay.RerollCardCost.ToString();
    }

    private void Update()
    {
        DisplayRerollButtonStatus();
    }

    private void DisplayRerollButtonStatus()
    {
        if (game.State.freeRoll > 0)
        {
            freeRollText.gameObject.SetActive(true);
            freeRollText.text = $"Free:{game.State.freeRoll}";
            costSlot.SetActive(false);
            rerollButton.interactable = true;
            rerollButtonEnergyLoad.fillAmount = 0;
        }
        else
        {
            freeRollText.gameObject.SetActive(false);
            costSlot.SetActive(true);
            rerollButton.interactable = game.State.energy >= Configs.GamePlay.RerollCardCost;
            rerollButtonEnergyLoad.fillAmount = Mathf.Clamp01(1 - game.State.energy / Configs.GamePlay.RerollCardCost);
        }
    }

    public void OnClickReroll()
    {
        game.DoPayReroll();
    }
}
