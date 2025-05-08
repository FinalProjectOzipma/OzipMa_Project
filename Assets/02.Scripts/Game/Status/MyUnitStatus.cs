using System.Collections.Generic;

public class MyUnitStatus : StatusBase
{
    int primaryKey;
    List<DefaultTable.MyUnit> row;

    public MyUnitStatus() { }

    public MyUnitStatus(int PrimaryKey, List<DefaultTable.MyUnit> Row)
    {
        primaryKey = PrimaryKey;
        row = Row;

        Init();
        var result = Row[PrimaryKey];
        StatusInit();
    }

    public void StatusInit()
    {
        var result = row[primaryKey];
        Health.MaxValue = result.Health;
        Health.SetValue(result.Health);

        Attack.SetValue(result.Attack);
        Defence.SetValue(result.Defence);
        MoveSpeed.SetValue(result.MoveSpeed);

        AttackCoolDown.SetValue(result.AttackCoolDown);
        AttackRange.SetValue(result.AttackRange);
    }
}