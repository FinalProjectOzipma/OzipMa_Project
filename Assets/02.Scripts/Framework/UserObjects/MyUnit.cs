using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnit : UserObject, IGettable
{
    public AtkType AtkType { get; set; }
    public AbilityType AbilityType { get; set; }
    public T GetClassAddress<T>() where T : UserObject
    {
        return this as T;
    }

    public override void Init(int primaryKey, Sprite sprite)
    {
        var result = Util.TableConverter<DefaultTable.MyUnit>(Managers.Data.Datas[Enums.Sheet.MyUnit]);
        base.Init(primaryKey, sprite); 
        Name = result[primaryKey].Name;
        Description = result[primaryKey].Description;
        if (Status == null)
            Status = new MyUnitStatus(primaryKey, result);
        else
            (Status as MyUnitStatus).StatusInit();
        RankType = result[primaryKey].Rank;
        AtkType = result[primaryKey].AttackType;
        AbilityType = result[primaryKey].AbilityType;
    }

    public void AddHealth(float amount) => GetUpCasting<MyUnitStatus>().Health.AddValue(amount);
}
