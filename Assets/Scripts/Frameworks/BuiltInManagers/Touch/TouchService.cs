using Core;
using Lean.Touch;
using UnityEngine;

public class TouchService : MonoBehaviour, IManager
{
    //public delegate void TouchDownEvent(Vector2 touch);
    //public event TouchDownEvent touchDown;
    //public delegate void TouchUpdateEvent(Vector2 delta);
    //public event TouchDownEvent touchUpdate;
    //public delegate void TouchUpEvent(Vector2 touch);
    //public event TouchDownEvent touchUp;
    public void Init()
    {
        LeanTouch.OnFingerTap += LeanTouch_OnFingerTap;
    }
    public void StartUp() { }
    public void Cleanup()
    {
        LeanTouch.OnFingerTap -= LeanTouch_OnFingerTap;
    }
    private void LeanTouch_OnFingerTap(LeanFinger obj)
    {
        if (!obj.StartedOverGui && !obj.IsOverGui) OnTap();
    }
    RaycastHit hit = new RaycastHit();
    private void OnTap()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            var go = hit.collider.gameObject;
            var clickable = go.GetComponent<ITapable>();
            if (clickable != null) clickable.OnTap();
        }
    }
    #region Touching
    void OnEnable()
    {
        LeanTouch.OnFingerDown += FingerDown;
        LeanTouch.OnFingerUpdate += FingerUpdate;
        LeanTouch.OnFingerUp += FingerUp;
    }
    void OnDisable()
    {
        LeanTouch.OnFingerDown -= FingerDown;
        LeanTouch.OnFingerUpdate -= FingerUpdate;
        LeanTouch.OnFingerUp -= FingerUp;
    }
    LeanFinger currentFinger;
    void FingerDown(LeanFinger finger)
    {
        if (currentFinger == null && !finger.StartedOverGui && !finger.IsOverGui)
        {
            ActionService.Dispatch<TouchDownAction>(finger);
            currentFinger = finger;
        }
    }
    void FingerUpdate(LeanFinger finger)
    {
        if (finger == currentFinger) ActionService.Dispatch<TouchUpdateAction>(finger);
    }
    void FingerUp(LeanFinger finger)
    {
        if (finger == currentFinger)
        {
            ActionService.Dispatch<TouchUpAction>(finger);
            currentFinger = null;
        }
    }
    #endregion
}
public interface ITapable
{
    void OnTap();
}