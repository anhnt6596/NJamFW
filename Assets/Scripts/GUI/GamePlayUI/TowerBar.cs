using Core;
using UnityEngine;
public class TowerBar : MonoBehaviour
{
    private void OnEnable()
    {
        ActionService.Sub<TouchDownAction>(OnTouchDown);
    }

    private void OnDisable()
    {
        ActionService.Unsub<TouchDownAction>(OnTouchDown);
    }

    private void OnTouchDown(TouchDownAction obj)
    {
        Vector3 screenPos = obj.Finger.ScreenPosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));

        worldPos.z = 0;

        App.Get<GameManager>().RunningGame.TryPlaceTower(worldPos);
    }
}