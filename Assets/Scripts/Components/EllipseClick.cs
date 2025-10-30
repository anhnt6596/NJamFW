using UnityEngine;

public class EllipseClick : MonoBehaviour
{
    [SerializeField] private float radiusX = 0.5f;
    [SerializeField] private float radiusY = 0.5f;

    public bool CheckClick(Vector3 screenPos)
    {
        var cam = Camera.main;
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos = PutPointOnMyZPlane(worldPos, cam);

        Vector3 local = transform.InverseTransformPoint(worldPos);

        return IsInsideEllipse(local);
    }

    private bool IsInsideEllipse(Vector3 local)
    {
        float val =
            (local.x * local.x) / (radiusX * radiusX) +
            (local.y * local.y) / (radiusY * radiusY);

        return val <= 1f;
    }

    private Vector3 PutPointOnMyZPlane(Vector3 pointFromCam, Camera cam)
    {
        if (!cam.orthographic)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            float myZ = transform.position.z;
            float t = (myZ - ray.origin.z) / ray.direction.z;
            return ray.GetPoint(t);
        }
        else
        {
            pointFromCam.z = transform.position.z;
            return pointFromCam;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        int segments = 30;
        Vector3 prev = Vector3.zero;
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 next = new Vector3(Mathf.Cos(angle) * radiusX, Mathf.Sin(angle) * radiusY, 0f);
            if (i > 0)
                Gizmos.DrawLine(transform.TransformPoint(prev), transform.TransformPoint(next));
            prev = next;
        }
    }
}