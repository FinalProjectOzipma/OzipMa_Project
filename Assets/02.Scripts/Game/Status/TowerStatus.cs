using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;
using static UnityEngine.Rendering.DebugUI.Table;

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
        Grade.SetValue(0);
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
