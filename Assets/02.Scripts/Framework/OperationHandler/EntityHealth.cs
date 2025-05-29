using Newtonsoft.Json;
using System;
using UnityEngine;

public class EntityHealth : FloatBase
{
    [JsonIgnore]
    public Action<float, float> OnChangeHealth;
    private EntityController controller;
    public float MaxValue { get; set; }

    public override void AddValue(float amount)
    {
        if (Managers.Game.IsGodMode) return;
        if (amount < 0)
        {
            base.AddValue(amount);
        }
        else
        {
            SetValue(Mathf.Min(MaxValue, amount + Value));
        }
        OnChangeHealth?.Invoke(Value, MaxValue);
    }
    public override void SetValue(float amount)
    {
        base.SetValue(amount);
        OnChangeHealth?.Invoke(Value, MaxValue);
    }
}
