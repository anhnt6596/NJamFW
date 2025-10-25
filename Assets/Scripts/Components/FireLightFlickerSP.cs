using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FireLightFlickerSR : FireLightFlicker
{
    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void SetColor(Color c)
    {
        sr.color = c;
    }
}
