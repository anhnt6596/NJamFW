using UnityEngine;

public enum Dir4
{
    Unknown = -1,
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}

public enum Dir2
{
    Unknown = -1,
    Left = 0,
    Right = 1,
}

public static class MovingUtils
{
    public static Dir2 GetDirection2Index(Vector3 dir, Transform cam, float deadZone = 0.05f)
    {
        dir.y = 0f;

        if (dir.sqrMagnitude < Mathf.Epsilon) return Dir2.Unknown;

        Vector3 move = dir;
        move.y = 0;
        move.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0;
        if (camRight.sqrMagnitude < 0.0001f)
            camRight = Vector3.right;
        else
            camRight.Normalize();

        float dot = Vector3.Dot(move, camRight);

        if (Mathf.Abs(dot) < deadZone) return Dir2.Unknown;

        return dot > 0 ? Dir2.Right : Dir2.Left;
    }

    public static Dir4 GetDirection4Index(Vector3 dir, Transform cam)
    {
        dir.y = 0f;

        if (dir.sqrMagnitude < Mathf.Epsilon)
            return Dir4.Unknown;

        GetCameraBasisXZ(cam, out var fwdXZ, out var rightXZ);

        float zC = Vector3.Dot(dir.normalized, fwdXZ);
        float xC = Vector3.Dot(dir.normalized, rightXZ);

        float angle = Mathf.Atan2(xC, zC) * Mathf.Rad2Deg;

        if (angle < 0) angle += 360f;

        if (angle >= 45f && angle < 135f)
            return Dir4.Right;
        else if (angle >= 135f && angle < 225f)
            return Dir4.Down;
        else if (angle >= 225f && angle < 315f)
            return Dir4.Left;
        else
            return Dir4.Up;
    }

    static void GetCameraBasisXZ(Transform cam, out Vector3 fwdXZ, out Vector3 rightXZ)
    {
        Vector3 f = cam.forward;
        f.y = 0f;

        // cam top down
        if (f.sqrMagnitude < 1e-6f)
        {
            float yaw = cam.eulerAngles.y * Mathf.Deg2Rad;
            f = new Vector3(Mathf.Sin(yaw), 0f, Mathf.Cos(yaw));
        }
        fwdXZ = f.normalized;

        rightXZ = new Vector3(fwdXZ.z, 0f, -fwdXZ.x);
    }
}