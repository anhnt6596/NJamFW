using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.U2D;

public abstract class Unit : MonoBehaviour
{
    public float HP;
    public abstract float speed { get; }
    public abstract float maxHP { get; }
    public DeffenseStats def;
    public bool isDead => HP <= 0;
    public Transform healthNode;
    public List<UnitStatus> statusList;

    public System.Action<Unit> OnDeath;
    public void TakeDamage(Damage dmgInput)
    {
        if (HP <= 0) return;
        var damageTake = GamePlayUtils.CalculateDamage(dmgInput, def);
        HP -= damageTake.amount;
        if (HP <= 0) Die();
    }

    protected void Die()
    {
        if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
        {
            var text = dieText[UnityEngine.Random.Range(0, dieText.Count)];
            App.Get<GUIEffectManager>().ShowScreenTextWP(text, healthNode.position, Color.white);
        }
        OnDeath?.Invoke(this);
    }

    protected List<string> dieText = new List<string>()
    {
        "!",
        "?",
        "A",
        "No Way",
        "Avenge me!",
        "Good Bye",
    };
}
