using Core;
using Lean.Touch;
using UnityEngine;

public class TouchService : MonoBehaviour, IManager
{
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
        LeanTouch.OnFingerTap += FingerTap;
    }
    void OnDisable()
    {
        LeanTouch.OnFingerDown -= FingerDown;
        LeanTouch.OnFingerUpdate -= FingerUpdate;
        LeanTouch.OnFingerUp -= FingerUp;
        LeanTouch.OnFingerTap -= FingerTap;
    }
    private int? activeFingerId = null;
    void FingerDown(LeanFinger finger)
    {
        if (activeFingerId == null && !finger.StartedOverGui && !finger.IsOverGui)
        {
            activeFingerId = finger.Index;
            ActionService.Dispatch<TouchDownAction>(finger);
        }
    }

    void FingerUpdate(LeanFinger finger)
    {
        if (activeFingerId == finger.Index)
        {
            ActionService.Dispatch<TouchUpdateAction>(finger);
        }
    }

    void FingerUp(LeanFinger finger)
    {
        if (activeFingerId == finger.Index)
        {
            ActionService.Dispatch<TouchUpAction>(finger);
            activeFingerId = null;
        }
    }

    void FingerTap(LeanFinger finger)
    {
        if (!finger.StartedOverGui && !finger.IsOverGui)
        {
            ActionService.Dispatch<TapAction>(finger);
        }
    }
    #endregion
}
public interface ITapable
{
    void OnTap();
}