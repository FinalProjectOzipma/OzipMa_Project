using System;

public class EntityHealth : FloatBase
{
    public Action<float, float> OnChangeHealth;
    private EntityController controller;
    private float maxHealth;

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public override void AddValue(float amount)
    {
        if (Managers.Game.IsGodMode) return;
        base.AddValue(amount);
        OnChangeHealth?.Invoke(Value, maxHealth);
    }
    public override void SetValue(float amount)
    {
        base.SetValue(amount);
        OnChangeHealth?.Invoke(Value, maxHealth);
    }
}
