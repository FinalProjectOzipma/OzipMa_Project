using System;

public class EntityHealth : FloatBase
{
    public Action<float> OnChangeHealth;

    public override void AddStat(float amount)
    {
        base.AddStat(amount);
        OnChangeHealth?.Invoke(Value);
    }
}
