using System;

public class EntityHealth : FloatBase
{
    public Action<float, float> OnChangeHealth;
    private EntityController controller;
    public float MaxValue { get; set; }

    public override void AddValue(float amount)
    {
        if (Managers.Game.IsGodMode) return;
        base.AddValue(amount);
        OnChangeHealth?.Invoke(Value, MaxValue);
    }
    public override void SetValue(float amount)
    {
        base.SetValue(amount);
        OnChangeHealth?.Invoke(Value, MaxValue);
    }
}
