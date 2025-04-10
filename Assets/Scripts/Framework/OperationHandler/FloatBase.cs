using UnityEngine;

public class FloatBase
{
    public float Value;
    public float ValueMultiples = 1.0f;

    public virtual void Init(float amount)
    {
        SetStat(amount);
    }

    public virtual void SetStat(float amount)
    {
        if (amount < 0.0f) return;
        Value = amount;
    }

    public virtual float GetStat()
    {
        return Value;
    }

    public virtual void AddStat(float amount)
    {
        SetStat(Mathf.Max(0, Value + amount));
    }

    public virtual void SetStatMultiples(float amount)
    {
        if (amount < 0) return;
        ValueMultiples = amount;
    }

    public virtual void AddMultiples(float amount)
    {
        SetStatMultiples(Mathf.Max(0, ValueMultiples + amount));
    }

    public virtual void MultiplesOperation()
    {
        SetStat(Mathf.Max(0, Value * ValueMultiples));
    }

    public virtual string GetValueToString()
    {
        return Value.ToString("F0");
    }
}
