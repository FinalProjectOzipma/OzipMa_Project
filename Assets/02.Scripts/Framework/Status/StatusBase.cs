using UnityEngine;

public class StatusBase
{
    public EntityHealth Health { get; set; } = new();

    public FloatBase Attack { get; set; } = new FloatBase();
    public FloatBase AttackCoolDown { get; set; } = new FloatBase();
    public FloatBase AttackRange { get; set; } = new FloatBase();
    public FloatBase Defence { get; set; } = new FloatBase();

    public FloatBase MoveSpeed { get; set; } = new FloatBase();

    public IntegerBase Level { get; set; } = new IntegerBase();
    public IntegerBase Stack { get; set; } = new IntegerBase();
    public IntegerBase MaxStack { get; set; } = new IntegerBase();
    public IntegerBase MaxLevel { get; set; } = new IntegerBase();
    public IntegerBase Grade { get; set; } = new IntegerBase();

    public StatusBase()
    {
        Init();
    }

    public void Init()
    {
        Level.SetValue(1);
        Stack.SetValue(0);
        MaxStack.SetValue(10);
        MaxLevel.SetValue(10);
        Grade.SetValue(1);
    }

    public void InitHealth() => Health.SetValue(Health.MaxValue);
    public void AddHealth(float amount, GameObject go) => Health.AddValue(amount);
}
