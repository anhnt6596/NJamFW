using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Unit : MonoBehaviour
{
    public float HP;
    public abstract float speed { get; }
    public abstract float maxHP { get; }
    public abstract Vector2 attackRange { get; }
    public abstract float attackSpeed { get; }
    public DeffenseStats def;
    public bool isDead => HP <= 0;

    public List<UnitStatus> statusList;

    #region Visual
    public Transform healthNode;
    public Transform burningNode;

    protected GameObject burnEffect;
    #endregion Visual

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
        statusList.Clear();
        DisplayBurnEffect(false);
        if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
        {
            var text = dieText[UnityEngine.Random.Range(0, dieText.Count)];
            App.Get<GUIEffectManager>().ShowScreenTextWP(text, healthNode.position, Color.white);
        }
        OnDeath?.Invoke(this);
    }

    public void AddStatus(UnitStatus newStatus)
    {
        var found = statusList.FirstOrDefault(s => s.type == newStatus.type);
        if (found == null) statusList.Add(newStatus);
        else
        {
            for (int i = 0; i < found.@params.Count; i++)
            {
                // hard code, status co param cao hon se duoc ap dung
                if (found.@params[0] > newStatus.@params[0]) found.@params[0] = newStatus.@params[0];
            }
        }
    }

    protected virtual void ApplyUpdateStatus()
    {
        for (int i = statusList.Count; i > 0; i--)
        {
            if (isDead) break;
            var status = statusList[i - 1];
            switch (status.type)
            {
                case UnitStatusEnum.Burning:
                    {
                        DisplayBurnEffect(status.@params[0] > 0);
                        if (status.@params[0] <= 0) statusList.Remove(status);
                        status.@params[0] -= Time.deltaTime;
                        TakeDamage(new Damage(status.@params[1] * Time.deltaTime, DamageEnum.True));
                        break;
                    }
                case UnitStatusEnum.TimeFrozen:
                    {
                        status.@params[0] -= Time.deltaTime;
                        if (status.@params[0] <= 0) statusList.Remove(status);
                        break;
                    }
            }
        }
    }

    private void DisplayBurnEffect(bool display)
    {
        if ((burnEffect != null) == display) return;
        else if (display)
        {
            if (isDead) return;
            burnEffect = LeanPool.Spawn(ResourceProvider.Effect.burning, burningNode, false);
            burnEffect.transform.localPosition = Vector3.zero;
            burnEffect.transform.localScale = Vector3.one;
        }
        else
        {
            burnEffect.transform.SetParent(null);
            LeanPool.Despawn(burnEffect);
            burnEffect = null;
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
