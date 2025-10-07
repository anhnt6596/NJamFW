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

    public static Damage operator +(Damage d, float value)
    {
        return new Damage(d.amount + value, d.type);
    }

    public static Damage operator +(Damage d1, Damage d2)
    {
        return new Damage(d1.amount + d2.amount, d1.type);
    }

    public static Damage operator -(Damage d, float value)
    {
        return new Damage(d.amount - value, d.type);
    }

    public static Damage operator *(Damage d, float value)
    {
        return new Damage(d.amount * value, d.type);
    }

    public static Damage operator /(Damage d, float value)
    {
        return new Damage(d.amount / value, d.type);
    }

    public override string ToString()
    {
        return $"Damage: {amount} ({type})";
    }
}