using System;
using UnityEngine;

public class OnTower : MonoBehaviour
{
    public Tower Tower { get; set; }
    public bool Ready { get; protected set; }

    public virtual void Setup(Tower tower)
    {
        Tower = tower;
        Ready = true;
    }

    public void LeaveTower()
    {
        Tower = null;
        Ready = false;
    }
}