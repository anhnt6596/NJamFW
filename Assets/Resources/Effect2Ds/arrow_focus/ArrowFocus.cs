using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFocus : MonoBehaviour
{
    [SerializeField] Transform target;

    Vector3 startPos, startScale;
    private void Awake()
    {
        startPos = target.localPosition;
        startScale = target.localScale;
    }

    private void OnEnable()
    {
        BounceDownPointer();
    }

    public void BounceDownPointer(float moveDistance = 0.3f, float moveDuration = 0.6f, float scaleAmount = 1.1f)
    {
        target.DOKill(true);;

        target.DOLocalMoveY(startPos.y + moveDistance, moveDuration)
              .SetEase(Ease.InOutSine)
              .SetLoops(-1, LoopType.Yoyo);

        target.DOScale(startScale * scaleAmount, moveDuration)
              .SetEase(Ease.InOutSine)
              .SetLoops(-1, LoopType.Yoyo);
    }
}
