public class TowerStatus : StatusBase
{
    public int PrimaryKey { get; set; }
    public TowerStatus(int id)
    {
        PrimaryKey = id;
        var Row = Managers.Data.Datas[Enums.Sheet.Tower][id] as DefaultTable.Tower;
        Attack.SetValue(Row.AttackDamage);
        AttackCoolDown.SetValue(Row.AttackCoolDown);
        AttackRange.SetValue(Row.AttackRange);

        // TODO : 사용자 데이터 필요 ----------
        Grade.SetValue(1);
        Level.SetValue(1);
        Stack.SetValue(0);
        MaxStack.SetValue(10);
        MaxLevel.SetValue(10);
        // ----------------------------------
    }

    /// <summary>
    /// 데이터만 새로 세팅하기 위한 함수
    /// </summary>
    /// <param name="newData">바꿀 데이터가 들어있는 TowerStatus</param>
    public void SetDatas(TowerStatus newData)
    {
        Attack.SetValue(newData.Attack.Value);
        Attack.SetValueMultiples(newData.Attack.ValueMultiples);
        Attack.SetGradeMultiple(newData.Attack.GradeMulitpes);
        Attack.SetResearchMultiple(newData.Attack.ResearchMultiples);


        AttackCoolDown.SetValue(newData.AttackCoolDown.Value);
        AttackCoolDown.SetValueMultiples(newData.AttackCoolDown.ValueMultiples);
        AttackCoolDown.SetGradeMultiple(newData.AttackCoolDown.GradeMulitpes);
        AttackCoolDown.SetResearchMultiple(newData.AttackCoolDown.ResearchMultiples);

        AttackRange.SetValue(newData.AttackRange.Value);
        AttackRange.SetValueMultiples(newData.AttackRange.ValueMultiples);
        AttackRange.SetGradeMultiple(newData.AttackRange.GradeMulitpes);
        AttackRange.SetResearchMultiple(newData.AttackRange.ResearchMultiples);

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
