using System.Collections.Generic;

public class MyUnitStatus : StatusBase
{
    public MyUnitStatus() { }
    public MyUnitStatus(int PrimaryKey, List<DefaultTable.MyUnit> Row)
    {
        Init();
        var result = Row[PrimaryKey];

        Health.MaxValue = result.Health;
        Health.SetValue(result.Health);

        Attack.SetValue(result.Attack);
        Defence.SetValue(result.Defence);
        MoveSpeed.SetValue(result.MoveSpeed);

        AttackCoolDown.SetValue(result.AttackCoolDown);
        AttackRange.SetValue(result.AttackRange);
    }
}
