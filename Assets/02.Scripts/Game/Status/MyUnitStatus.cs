using System.Collections.Generic;

public class MyUnitStatus : StatusBase
{
    public EntityHealth Health = new();
    public float MaxHealth;

    public FloatBase Defence = new();
    public FloatBase MoveSpeed = new();

    public MyUnitStatus(int PrimaryKey, List<DefaultTable.MyUnit> Row)
    {
        Init();
        var result = Row[PrimaryKey];

        Health.SetValue(result.Health);
        MaxHealth = Health.GetValue();

        Attack.SetValue(result.Attack);
        Defence.SetValue(result.Defence);
        MoveSpeed.SetValue(result.MoveSpeed);

        AttackCoolDown.SetValue(result.AttackCoolDown);
        AttackRange.SetValue(result.AttackRange);
    }
}
