using UnityEngine;

[ExecuteAlways]
public class FollowPosition : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        if (target != null)
            transform.position = target.position;
    }
}