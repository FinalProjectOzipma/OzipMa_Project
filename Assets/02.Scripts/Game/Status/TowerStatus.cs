using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;

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
}
