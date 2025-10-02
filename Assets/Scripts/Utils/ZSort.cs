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
        Vector3 pos = transform.position;
        pos.z = pos.y * zOffsetFactor + extraOffset;
        transform.position = pos;
    }
}