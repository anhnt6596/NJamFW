using Core;
using Lean.Touch;
using UnityEngine;

public class TouchDownAction : IAction
{
    public LeanFinger Finger { get; set; }
    public virtual void SetData(object[] _params)
    {
        Finger = (LeanFinger)_params[0];
    }
}
public class TouchUpdateAction : IAction
{
    public LeanFinger Finger { get; set; }
    public virtual void SetData(object[] _params)
    {
        Finger = (LeanFinger)_params[0];
    }
}
public class TouchUpAction : IAction
{
    public LeanFinger Finger { get; set; }
    public virtual void SetData(object[] _params)
    {
        Finger = (LeanFinger)_params[0];
    }
}
public class TapAction : IAction
{
    public LeanFinger Finger { get; set; }
    public virtual void SetData(object[] _params)
    {
        Finger = (LeanFinger)_params[0];
    }
}