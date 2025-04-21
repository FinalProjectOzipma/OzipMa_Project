using System.Collections.Generic;

public class MyUnitStatus : StatusBase
{
    public EntityHealth Health = new();
    public FloatBase MaxHealth;

    public FloatBase Defences = new();
    public FloatBase MoveSpeed = new();

    public MyUnitStatus(int PrimaryKey, List<DefaultTable.MyUnit> Row)
    {
        Init();
        var result = Row[PrimaryKey];

        Health.SetValue(result.Health);
        MaxHealth.SetValue(Health.GetValue());

        Attack.SetValue(result.Attack);
        Defences.SetValue(Health.GetValue());
        MoveSpeed.SetValue(result.MoveSpeed);

        AttackCoolDown.SetValue(result.AttackCoolDown);
        AttackRange.SetValue(result.AttackRange);

        Level.SetValue(1);
        Stack.SetValue(0);
        MaxStack.SetValue(20);
        MaxLevel.SetValue(20);
        Grade.SetValue(0);
    }
}
