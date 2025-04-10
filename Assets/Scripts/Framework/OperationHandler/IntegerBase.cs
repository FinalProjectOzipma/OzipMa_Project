using UnityEngine;

public class IntegerBase
{
    public int Value;
    public int ValueMultiples = 1;

    public virtual void Init(int amount)
    {
        SetStat(amount);
    }

    public virtual void SetStat(int amount)
    {
        if (amount < 0.0f) return;
        Value = amount;
    }

    public virtual float GetStat()
    {
        return Value;
    }

    public virtual void AddStat(int amount)
    {
        SetStat(Mathf.Max(0, Value + amount));
    }

    public virtual void SetStatMultiples(int amount)
    {
        if (amount < 0) return;
        ValueMultiples = amount;
    }

    public virtual void AddMultiples(int amount)
    {
        SetStatMultiples(Mathf.Max(0, ValueMultiples + amount));
    }

    public virtual void MultiplesOperation()
    {
        SetStat(Mathf.Max(0, Value * ValueMultiples));
    }

    public virtual string GetValueToString()
    {
        return Value.ToString("N2");
    }
}
