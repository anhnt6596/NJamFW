using System;

[Serializable]
public struct Damage
{
    public Damage(float amount, DamageEnum type)
    {
        this.amount = amount;
        this.type = type;
    }
    public float amount;
    public DamageEnum type;
}