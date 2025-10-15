using Core;
using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;
using static CW.Common.CwInputManager;

[ExecuteAlways]
public class CameraController2D : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float mouseZoomSpeed = 5f;
    public float touchZoomSpeed = 0.01f;
    public float minZoom = 3f;
    public float maxZoom = 10f;

    [Header("Pan Settings")]
    public float dragSpeed = 1f;
    public bool invertDrag = true;

    [Header("Camera Bounds (World Space)")]
    public Rect bounds = new Rect(-10, -10, 20, 20);

    private Camera cam;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void OnEnable()
    {
        ActionService.Sub<TouchDownAction>(OnTouchDown);
        ActionService.Sub<TouchUpdateAction>(OnTouchUpdate);
        ActionService.Sub<TouchUpAction>(OnTouchUp);
    }

    private void OnDisable()
    {
        ActionService.Unsub<TouchDownAction>(OnTouchDown);
        ActionService.Unsub<TouchUpdateAction>(OnTouchUpdate);
        ActionService.Unsub<TouchUpAction>(OnTouchUp);
    }

    private void Update()
    {
        if (!Application.isPlaying)
            return;

        HandleMouseZoom();
        ClampToBounds();
    }

    private Vector3 lastTouchPos;
    private void OnTouchDown(TouchDownAction action)
    {
        lastTouchPos = action.Finger.ScreenPosition;
    }

    private void OnTouchUpdate(TouchUpdateAction action)
    {
        HandleTouchZoom();
        Vector3 delta = action.Finger.ScreenDelta;
        delta *= (invertDrag ? 1 : -1);

        Vector3 move = cam.ScreenToWorldPoint(new Vector3(delta.x, delta.y, 0))
                        - cam.ScreenToWorldPoint(Vector3.zero);

        transform.position -= move * dragSpeed;
        lastTouchPos = Input.mousePosition;
    }

    private void OnTouchUp(TouchUpAction action)
    {
        zoomDistance = -1f;
    }

    // --- ZOOM (scroll mouse) ---
    private void HandleMouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.001f)
        {
            cam.orthographicSize -= scroll * mouseZoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    // --- ZOOM (Touching) ---
    private float zoomDistance = -1f;
    private void HandleTouchZoom()
    {
        var fingers = LeanTouch.GetFingers(true, true);
        if (fingers.Count >= 2)
        {
            var finger1 = fingers[0];
            var finger2 = fingers[1];

            var pos1 = finger1.ScreenPosition;
            var pos2 = finger2.ScreenPosition;

            float currentDistance = Vector2.Distance(pos1, pos2);

            if (zoomDistance > 0)
            {
                float delta = zoomDistance - currentDistance;

                float newSize = cam.orthographicSize + delta * touchZoomSpeed;
                cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            }

            zoomDistance = currentDistance;
        }
        else
        {
            zoomDistance = -1f;
        }
    }

    // --- Camera Bounds ---
    private void ClampToBounds()
    {
        if (cam == null) return;

        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        float minX = bounds.xMin + horzExtent;
        float maxX = bounds.xMax - horzExtent;
        float minY = bounds.yMin + vertExtent;
        float maxY = bounds.yMax - vertExtent;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
#endif
}
