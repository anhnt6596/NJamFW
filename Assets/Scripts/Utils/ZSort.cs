using UnityEngine;

public class ZSort : MonoBehaviour
{
    public float zOffsetFactor = 0.01f;
    public float extraOffset = 0f;
    public bool onlyAtStart = false;

    void Start()
    {
        if (onlyAtStart) ApplyZSort();
    }

    void LateUpdate()
    {
        if (!onlyAtStart) ApplyZSort();
    }

    void ApplyZSort()
    {
        transform.position = GamePlayUtils.Y2Z(transform.position, extraOffset);
    }
}