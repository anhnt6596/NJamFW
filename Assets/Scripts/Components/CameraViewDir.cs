using UnityEngine;

public class CameraViewDir : MonoBehaviour
{
    public static int CurrentViewDir2 { get; private set; } = 0;
    public static int CurrentViewDir8 { get; private set; } = 0;

    public static bool TransformChanged { get; private set; } = false;
    public static Vector3? CamForward { get; private set; }


    private void Update()
    {
        // update direction
        float angleY = transform.eulerAngles.y;
        CurrentViewDir8 = MathUtils.GetViewType8(angleY);
        CurrentViewDir2 = CurrentViewDir8 % 2;

        // update last pos/rot
        var curForward = transform.forward;

        if (curForward != CamForward)
        {
            TransformChanged = true;
            CamForward = curForward;
        }
        else
        {
            TransformChanged = false;
        }
    }
}