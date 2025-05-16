using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;

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

    public void InvenStatus()
    {
        MyUnit result = Managers.Player.Inventory.GetItem<MyUnit>(primaryKey);
        Health.MaxValue = result.Status.Health.GetValue();
        Health.SetValue(Health.MaxValue);

        Attack.SetValue(result.Status.Attack.Value);
        Defence.SetValue(result.Status.Defence.Value);
        MoveSpeed.SetValue(result.Status.MoveSpeed.Value);

        AttackCoolDown.SetValue(result.Status.AttackCoolDown.Value);
        AttackRange.SetValue(result.Status.AttackRange.Value);
    }
    
    /// <summary>
    /// 파이어베이스에 저장된 데이터를 로드해서 데이터만 덮어씌우는 메서드
    /// </summary>
    /// <param name="newData"></param>
    public void SetDatas(MyUnitStatus newData)
    {
        Health.MaxValue = newData.Health.MaxValue;
        Health.SetValue(newData.Health.Value);
        Health.SetValueMultiples(newData.Health.ValueMultiples);
        Defence.SetValue(newData.Defence.Value);
        Defence.SetValueMultiples(newData.Defence.ValueMultiples);
        MoveSpeed.SetValue(newData.MoveSpeed.Value);
        MoveSpeed.SetValueMultiples(newData.Defence.ValueMultiples);

        Attack.SetValue(newData.Attack.Value);
        Attack.SetValueMultiples(newData.Attack.ValueMultiples);
        AttackCoolDown.SetValue(newData.AttackCoolDown.Value);
        AttackCoolDown.SetValueMultiples(newData.AttackCoolDown.ValueMultiples);
        AttackRange.SetValue(newData.AttackRange.Value);
        AttackRange.SetValueMultiples(newData.AttackRange.ValueMultiples);

        Grade.SetValue(newData.Grade.Value);
        Grade.SetValueMultiples(newData.Grade.ValueMultiples);
        Level.SetValue(newData.Level.Value);
        Level.SetValueMultiples(newData.Level.ValueMultiples);
        Stack.SetValue(newData.Stack.Value);
        Stack.SetValueMultiples(newData.Stack.ValueMultiples);
        MaxStack.SetValue(newData.MaxStack.Value);
        MaxStack.SetValueMultiples(newData.MaxStack.ValueMultiples);
        MaxLevel.SetValue(newData.MaxLevel.Value);
        MaxLevel.SetValueMultiples(newData.MaxLevel.ValueMultiples);
    }
}