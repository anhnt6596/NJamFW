using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    private static Camera mainCam;
    private static Vector3 originalPos;

    void Awake()
    {
        if (mainCam == null)
            mainCam = Camera.main;
        if (mainCam != null)
            originalPos = mainCam.transform.localPosition;
    }

    /// <summary>
    /// L?c camera b?ng DOTween.
    /// </summary>
    /// <param name="duration">Th?i gian l?c (giây)</param>
    /// <param name="strength">?? m?nh c?a l?c (m?c ??nh 0.3f)</param>
    /// <param name="vibrato">T?n s? dao ??ng (m?c ??nh 10)</param>
    /// <param name="randomness">?? ng?u nhiên h??ng (m?c ??nh 90)</param>
    public static void Shake(float duration = 0.3f, float strength = 0.3f, int vibrato = 10, float randomness = 90f)
    {
        if (mainCam == null) return;

        // Reset n?u ?ang shake d?
        mainCam.transform.DOKill();
        mainCam.transform.localPosition = originalPos;

        // G?i hi?u ?ng shake
        mainCam.transform.DOShakePosition(duration, strength, vibrato, randomness)
            .OnComplete(() =>
            {
                // Tr? v? v? trí ban ??u sau khi shake xong
                mainCam.transform.localPosition = originalPos;
            });
    }
}