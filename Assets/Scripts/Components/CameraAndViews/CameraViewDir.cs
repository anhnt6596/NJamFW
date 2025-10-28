using UnityEngine;

public class CameraViewDir : MonoBehaviour
{
    public static int CurrentViewDir2 { get; private set; } = 0;
    public static int CurrentViewDir8 { get; private set; } = 0;

    public static bool TransformChanged { get; private set; } = false;
    public static Vector3 CamForward { get; private set; }

    private void OnEnable()
    {
        UpdateCamInfo();
    }

    private void Update()
    {
        var curForward = transform.forward;

        if (curForward != CamForward)
        {
            TransformChanged = true;
            UpdateCamInfo();
        }
        else
        {
            TransformChanged = false;
        }
    }

    private void UpdateCamInfo()
    {
        // update forward vector
        CamForward = transform.forward;

        // update direction
        float angleY = transform.eulerAngles.y;
        CurrentViewDir8 = MathUtils.GetViewType8(angleY);
        CurrentViewDir2 = CurrentViewDir8 % 2;
    }
}