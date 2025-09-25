using UnityEngine;

public abstract class BaseGUI : MonoBehaviour
{
    public abstract bool IsActive { get; set; }
    public abstract GUILayer Layer { get; }

    public virtual void Show()
    {
        IsActive = true;
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        IsActive = false;
        gameObject.SetActive(false);
    }
}