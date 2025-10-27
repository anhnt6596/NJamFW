using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ViewDir2 : MonoBehaviour
{
    [SerializeField] Sprite sprite0;
    [SerializeField] Sprite sprite45;
    SpriteRenderer sr;
    private int lastViewDir = -1;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        var cam = Camera.main;
        if (!cam) return;

        int viewDir = CameraViewDir.CurrentViewDir2;

        if (viewDir == lastViewDir) return;
        lastViewDir = viewDir;

        sr.sprite = (viewDir == 0) ? sprite0 : sprite45;
    }
}
