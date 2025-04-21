using System;

public class EntityHealth : FloatBase
{
    public Action<float> OnChangeHealth;

    public override void AddValue(float amount)
    {
        base.AddValue(amount);
        Util.Log($"{Value}");
        OnChangeHealth?.Invoke(Value);
    }
}
