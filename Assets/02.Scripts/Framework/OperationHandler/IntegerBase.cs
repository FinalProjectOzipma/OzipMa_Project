using UnityEngine;

public class IntegerBase
{
    public int Value;
    public int ValueMultiples = 1;

    public virtual void Init(int amount)
    {
        SetValue(amount);
    }

    public virtual void SetValue(int amount)
    {
        if (amount < 0.0f) return;
        Value = amount;
    }

    public virtual float GetValue()
    {
        return Value;
    }

    public virtual void AddValue(int amount)
    {
        SetValue(Mathf.Max(0, Value + amount));
    }

    public virtual void SetValueMultiples(int amount)
    {
        if (amount < 0) return;
        ValueMultiples = amount;
    }

    public virtual void AddMultiples(int amount)
    {
        SetValueMultiples(Mathf.Max(0, ValueMultiples + amount));
    }

    public virtual void MultiplesOperation()
    {
        SetValue(Mathf.Max(0, Value * ValueMultiples));
    }

    public virtual string GetValueToString()
    {
        return Value.ToString("N2");
    }
}
