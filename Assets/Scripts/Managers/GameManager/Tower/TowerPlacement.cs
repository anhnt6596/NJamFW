using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    private bool isHighlighed = false;
    private bool placed = false;
    
    public bool IsHighlighted => isHighlighed;
    public bool Placed => placed;

    public void SetHighlight(bool status)
    {
        isHighlighed = status;
        // enable or disable highlight effect
    }

    public void SetPlaced(bool status)
    {
        placed = status;
    }

    public Vector2 GetTowerAdjustPosition()
    {
        // adjust later
        return transform.position;
    }

    public bool InTouchCollision(Vector3 position)
    {
        return true;
    }
}