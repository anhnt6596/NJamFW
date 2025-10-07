using Core;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

public class GUIEffectManager : MonoBehaviour, IManager
{
    [SerializeField] ScreenText screenTextPrefab;
    [SerializeField] Image flashImage;
    GUIManager _guiMgr;
    public void Init() { }

    public void StartUp()
    {
        _guiMgr = App.Get<GUIManager>();
        flashImage.gameObject.SetActive(false);
    }

    public void Cleanup() { }

    public ScreenText ShowScreenTextWP(string content, Vector3 wPos, Color color, GUILayer layer = GUILayer.GUI)
    {
        var screenPos = Camera.main.WorldToScreenPoint(wPos);
        return ShowScreenText(content, screenPos, color, layer);
    }

    public ScreenText ShowScreenText(string content, Vector3 screenPos, Color color, GUILayer layer = GUILayer.GUI)
    {
        var text = LeanPool.Spawn(screenTextPrefab);
        var guiLayer = _guiMgr.GetLayer(layer);
        text.transform.parent = guiLayer;

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(guiLayer, screenPos, Camera.main, out uiPos);
        text.transform.position = screenPos;

        text.Show(content, color, () => LeanPool.Despawn(text));
        return text;
    }

    public Image ShowInvalidEffect(Vector3 wPos, GUILayer layer = GUILayer.GUI)
    {
        var screenPos = Camera.main.WorldToScreenPoint(wPos);
        var invalid = LeanPool.Spawn(ResourceProvider.Effect.invalid);
        var guiLayer = _guiMgr.GetLayer(layer);
        invalid.transform.parent = guiLayer;

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(guiLayer, screenPos, Camera.main, out uiPos);
        invalid.transform.position = screenPos;

        invalid.SetAlpha(0);
        invalid.transform.localScale = Vector3.one * 0.5f;
        var seq = DOTween.Sequence();
        seq.Append(invalid.DOFade(1, 0.2f));
        seq.Join(invalid.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack));
        seq.Append(invalid.DOFade(0, 0.6f));
        seq.AppendCallback(() => LeanPool.Despawn(invalid));
        return invalid;
    }

    private Tween flashScreenTween;
    public void FlashScreen(Color color, float duration = 0.4f)
    {
        if (flashImage == null) return;

        flashScreenTween?.Kill();

        flashImage.gameObject.SetActive(true);
        flashImage.color = new Color(color.r, color.g, color.b, 0);

        flashScreenTween = flashImage.DOFade(color.a, duration / 4)
            .OnComplete(() =>
            {
                flashImage.DOFade(0f, duration * 3 / 4)
                    .OnComplete(() => flashImage.gameObject.SetActive(false));
            });
    }
}
