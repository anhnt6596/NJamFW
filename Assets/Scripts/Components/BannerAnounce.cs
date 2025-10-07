using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BannerAnnounce : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CanvasGroup group;                 // CanvasGroup c?a toàn banner
    [SerializeField] private RectTransform textRect;            // Rect c?a Text
    [SerializeField] private TextMeshProUGUI textTMP;           // TextMeshProUGUI hi?n th? ch?
    [SerializeField] private RectTransform borderTopLeft;       // ?nh border góc trên trái
    [SerializeField] private RectTransform borderBottomRight;   // ?nh border góc d??i ph?i

    [Header("Timing")]
    [SerializeField] private float enterDuration = 0.45f;       // th?i gian l??t vào
    [SerializeField] private float holdDuration = 0.9f;         // th?i gian gi? ? gi?a
    [SerializeField] private float exitDuration = 0.35f;        // th?i gian l??t ra
    [SerializeField] private float textPunchScale = 1.15f;      // scale “n?y” khi vào

    [Header("Slide Offsets (px)")]
    [SerializeField] private float slideFromLeft = 700f;        // border TL vào t? trái -> ph?i
    [SerializeField] private float slideFromRight = 700f;       // border BR vào t? ph?i -> trái
    [SerializeField] private float textSlideFrom = 400f;        // text vào t? trái -> ph?i (nh?)

    private Sequence _seq;
    private Vector2 _textRest, _tlRest, _brRest;

    void Awake()
    {
        if (group == null) group = GetComponent<CanvasGroup>();
        _textRest = textRect.anchoredPosition;
        _tlRest = borderTopLeft.anchoredPosition;
        _brRest = borderBottomRight.anchoredPosition;
        InstantHide();
    }

    /// <summary>
    /// Hi?n banner v?i message, ch?y hi?u ?ng: borders l??t vào + text pop, gi? 1 lúc r?i thoát.
    /// </summary>
    public void Show(string message)
    {
        // Kill c?
        _seq?.Kill();
        gameObject.SetActive(true);

        // Set message
        if (textTMP != null) textTMP.text = message;
        var startScale = textRect.localScale;

        // Reset tr??c khi vào
        group.alpha = 0f;
        textRect.anchoredPosition = _textRest + new Vector2(-textSlideFrom, 0f);
        textRect.localScale = startScale; // gi? ?úng scale g?c
        borderTopLeft.anchoredPosition = _tlRest + new Vector2(-slideFromLeft, 0f);
        borderBottomRight.anchoredPosition = _brRest + new Vector2(slideFromRight, 0f);

        // Sequence vào/gi?/ra
        _seq = DOTween.Sequence().SetUpdate(true).SetLink(gameObject);

        // ENTER
        _seq.Append(group.DOFade(1f, 0.15f));

        // Borders l??t vào song song
        _seq.Join(borderTopLeft.DOAnchorPos(_tlRest, enterDuration).SetEase(Ease.OutCubic));
        _seq.Join(borderBottomRight.DOAnchorPos(_brRest, enterDuration).SetEase(Ease.OutCubic));

        // Text l??t vào
        _seq.Join(textRect.DOAnchorPos(_textRest, enterDuration * 0.9f).SetEase(Ease.OutCubic));

        // ?? Scale: ph?ng lên r?i v? l?i **?úng lúc** borders k?t thúc (Yoyo trong enterDuration)
        _seq.Join(
            textRect.DOScale(startScale * textPunchScale, enterDuration * 0.5f)
                    .SetEase(Ease.OutBack, 1.3f)
                    .SetLoops(2, LoopType.Yoyo)
        );

        // (Không còn Append scale v? 1 n?a)

        // HOLD
        _seq.AppendInterval(holdDuration);

        // EXIT
        _seq.Append(group.DOFade(0f, exitDuration).SetEase(Ease.InQuad));
        _seq.Join(borderTopLeft.DOAnchorPos(_tlRest + new Vector2(-slideFromLeft, 0f), exitDuration).SetEase(Ease.InCubic));
        _seq.Join(borderBottomRight.DOAnchorPos(_brRest + new Vector2(slideFromRight, 0f), exitDuration).SetEase(Ease.InCubic));
        _seq.Join(textRect.DOAnchorPos(_textRest + new Vector2(textSlideFrom, 0f), exitDuration).SetEase(Ease.InCubic));

        _seq.OnComplete(() => InstantHide());
    }

    /// <summary>?n ngay l?p t?c (reset alpha, v? trí v? rest, t?t object).</summary>
    public void InstantHide()
    {
        _seq?.Kill();
        group.alpha = 0f;
        textRect.anchoredPosition = _textRest;
        borderTopLeft.anchoredPosition = _tlRest;
        borderBottomRight.anchoredPosition = _brRest;
        gameObject.SetActive(false);
    }
}
