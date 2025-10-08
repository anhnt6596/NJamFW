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
    [SerializeField] float coolDownTime = 1f;
    float remainCooldown = 0f;

    Game game;

    private void OnEnable()
    {
        game = App.Get<GameManager>().RunningGame;
        costText.text = Configs.GamePlay.RerollCardCost.ToString();
        Game.OnCardsRolled += OnCardRolled;
    }

    private void OnDisable()
    {
        Game.OnCardsRolled -= OnCardRolled;
    }

    private void OnCardRolled()
    {
        remainCooldown = coolDownTime;
    }

    private void Update()
    {
        if (remainCooldown > 0) remainCooldown -= Time.deltaTime;
        DisplayRerollButtonStatus();
    }

    private void DisplayRerollButtonStatus()
    {
        if (game.InputStateEnum != InputStateEnum.SelectingCard) rerollButton.interactable = false;
        if (game.State.freeRoll > 0)
        {
            freeRollText.gameObject.SetActive(true);
            freeRollText.text = $"Free:{game.State.freeRoll}";
            costSlot.SetActive(false);
            rerollButton.interactable = remainCooldown <= 0;
            rerollButtonEnergyLoad.fillAmount = 0;
        }
        else
        {
            freeRollText.gameObject.SetActive(false);
            costSlot.SetActive(true);
            rerollButton.interactable = game.State.energy >= Configs.GamePlay.RerollCardCost && remainCooldown <= 0;
            rerollButtonEnergyLoad.fillAmount = Mathf.Clamp01(1 - game.State.energy / Configs.GamePlay.RerollCardCost);
        }
    }

    public void OnClickReroll()
    {
        if (game.InputStateEnum != InputStateEnum.SelectingCard) return;
        if (remainCooldown > 0) return;

        remainCooldown = coolDownTime;
        game.DoPayReroll();
    }
}
