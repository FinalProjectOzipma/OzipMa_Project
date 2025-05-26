using System;
using UnityEngine;

public class FloatBase
{
    public float Value;
    public float ValueMultiples = 1.0f;
    public float GradeMulitpes = 1.0f;
    public float ResearchMultiples = 1.0f;
    public event Action<float> OnChangeValue;

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
        return Value * ValueMultiples * GradeMulitpes * ResearchMultiples;
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
        SetValue(Mathf.Max(0, Value * ValueMultiples * GradeMulitpes * ResearchMultiples));
    }

    public virtual void SetResearchMultiple(float amount)
    {
        if (amount < 0) return;
        ResearchMultiples = amount;
        OnChangeValue?.Invoke(GetValue());
    }


    public virtual void SetGradeMultiple(float amount)
    {
        if (amount < 0) return;
        GradeMulitpes = amount;
        OnChangeValue?.Invoke(GetValue());
    }

    public virtual string GetValueToString(string n = "N2")
    {
        return (Value * ValueMultiples * GradeMulitpes * ResearchMultiples).ToString(n);
    }

    public virtual string GetUpstatValueToString(string n = "N2")
    {
        return (GetValue() - Value).ToString(n);
    }
}
