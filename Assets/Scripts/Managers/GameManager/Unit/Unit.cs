using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public abstract class Unit : MonoBehaviour
{
    public float HP;
    public abstract float speed { get; }
    public abstract float maxHP { get; }
    public abstract Vector2 attackRange { get; }
    public abstract float attackSpeed { get; }
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

    protected virtual void ApplyUpdateStatus()
    {
        for (int i = statusList.Count; i > 0; i--)
        {
            var status = statusList[i - 1];
            switch (status.type)
            {
                //case UnitStatusEnum.Burning:
                //    if (s.@params.Count < 2) break;
                //    float burnDmg = s.@params[0];
                //    float burnDur = s.@params[1];
                //    TakeDamage(new Damage() { amount = burnDmg * Time.deltaTime, type = DamageType.Fire });
                //    s.@params[1] -= Time.deltaTime;
                //    if (s.@params[1] <= 0) s.@params[1] = 0;
                //    break;
                //case UnitStatusEnum.Poisoned:
                //    if (s.@params.Count < 2) break;
                //    float poisonDmg = s.@params[0];
                //    float poisonDur = s.@params[1];
                //    TakeDamage(new Damage() { amount = poisonDmg * Time.deltaTime, type = DamageType.Poison });
                //    s.@params[1] -= Time.deltaTime;
                //    if (s.@params[1] <= 0) s.@params[1] = 0;
                //    break;
                case UnitStatusEnum.TimeFrozen:
                    float frozenDur = status.@params[0];
                    status.@params[0] -= Time.deltaTime;
                    if (status.@params[0] <= 0) statusList.Remove(status);
                    break;
            }
        }
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
