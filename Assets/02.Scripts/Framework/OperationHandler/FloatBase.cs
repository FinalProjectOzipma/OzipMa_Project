using System;
using UnityEngine;

public class FloatBase
{
    public float Value;
    public float ValueMultiples = 1.0f;
    public Action<float> OnChangeValue;

    public virtual void Init(float amount)
    {
        SetValue(amount);
    }

    public virtual void SetValue(float amount)
    {
        if (amount < 0.0f) return;
        Value = amount;
        OnChangeValue?.Invoke(GetValue());
    }

    public virtual float GetValue()
    {
        return Value * ValueMultiples;
    }

    public virtual void AddValue(float amount)
    {
        SetValue(Mathf.Max(0, Value + amount));
    }

    public virtual void SetValueMultiples(float amount)
    {
        if (amount < 0) return;
        ValueMultiples = amount;
        OnChangeValue?.Invoke(GetValue());
    }

    public virtual void AddMultiples(float amount)
    {
        SetValueMultiples(Mathf.Max(0, ValueMultiples + amount));
    }

    public virtual void MultiplesOperation()
    {
        SetValue(Mathf.Max(0, Value * ValueMultiples));
    }

    public virtual string GetValueToString()
    {
        return Value.ToString("F0");
    }
}
