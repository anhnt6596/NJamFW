using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerObjectParent : MonoBehaviour
{
    [SerializeField] Transform[] children;
    [SerializeField] Transform[] followPos;
    [SerializeField] float offsetZ = 0.5f;
    private void LateUpdate()
    {
        if (CameraViewDir.TransformChanged)
            EvaluateChildrenPos();
    }

    private void EvaluateChildrenPos()
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (TryIntersectPos(followPos[i].position, transform, out var worldHit, out var localXY))
            {
                children[i].position = worldHit;
                children[i].localPosition += Vector3.back * offsetZ;
            }
        }
    }

    public static bool TryIntersectPos(
        Vector3 wPos,
        Transform allyParent,
        out Vector3 worldHit,
        out Vector2 localXY,
        Camera cam = null,
        bool considerInfiniteLine = false)
    {
        worldHit = default;
        localXY = default;

        if (!allyParent) return false;
        cam ??= Camera.main;
        if (!cam) return false;

        // --- DỰNG MẶT PHẲNG XY (local z=0) CỦA allyParent ---
        // Pháp tuyến = trục Z local đưa ra world (bỏ ảnh hưởng scale -> dùng rotation)
        Vector3 planeNormal = allyParent.rotation * Vector3.forward;
        // Một điểm nằm trên mặt phẳng local z=0 là gốc local -> đưa ra world
        Vector3 planePoint = allyParent.TransformPoint(Vector3.zero);

        var plane = new Plane(planeNormal, planePoint);

        // Nếu wPos đã nằm trên mặt phẳng (local z ~ 0) thì trả luôn
        if (Mathf.Approximately(plane.GetDistanceToPoint(wPos), 0f))
        {
            worldHit = wPos;
            Vector3 local = allyParent.InverseTransformPoint(worldHit);
            localXY = new Vector2(local.x, local.y);
            return true;
        }

        // --- TIA ĐI QUA wPos THEO HƯỚNG camera.forward ---
        Vector3 dir = cam.transform.forward.normalized;
        var ray = new Ray(wPos, dir);

        // Thử cắt theo hướng thuận (ray t >= 0)
        if (plane.Raycast(ray, out float enter) && enter >= 0f)
        {
            worldHit = ray.GetPoint(enter);
        }
        else
        {
            // Coi như đường thẳng vô hạn: bắn tia ngược hướng
            var rayBack = new Ray(wPos, -dir);
            if (plane.Raycast(rayBack, out float enterBack) && enterBack >= 0f)
            {
                worldHit = rayBack.GetPoint(enterBack);
            }
            else
            {
                return false;
            }
        }

        // Đổi sang local của allyParent để lấy (x,y)
        Vector3 localHit = allyParent.InverseTransformPoint(worldHit);
        localXY = new Vector2(localHit.x, localHit.y);
        return true;
    }
}
