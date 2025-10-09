using UnityEngine;

public class CameraFixWidth : MonoBehaviour
{
    [SerializeField] float orthographicSize = 10f;
    public float targetAspect = 16f / 9f;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        float currentAspect = (float)Screen.width / Screen.height;
        cam.orthographicSize = (targetAspect / currentAspect) * orthographicSize;
    }
}