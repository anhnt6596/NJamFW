using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    private Vector3 originalLocalPos;

    void Awake()
    {
        instance = this;
        originalLocalPos = transform.localPosition;
    }

    public static void Shake(float duration = 0.3f, float strength = 0.3f, int vibrato = 10, float randomness = 90f)
    {
        if (instance == null) return;

        Transform t = instance.transform;
        t.DOKill();
        t.localPosition = instance.originalLocalPos;

        t.DOShakePosition(duration, strength, vibrato, randomness)
            .OnComplete(() => t.localPosition = instance.originalLocalPos);
    }
}