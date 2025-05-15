using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MyUnit : UserObject, IGettable
{
    public AtkType AtkType { get; set; }
    public AbilityType AbilityType { get; set; }
    public T GetClassAddress<T>() where T : UserObject
    {
        return this as T;
    }

    public void Init(int primaryKey)
    {
        MyUnit unitdata = Managers.Player.Inventory.GetItem<MyUnit>(primaryKey);
        if (unitdata == null)
        {
            var result = Util.TableConverter<DefaultTable.MyUnit>(Managers.Data.Datas[Enums.Sheet.MyUnit]);
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
        else
        {
            Name = unitdata.Name;
            Description = unitdata.Description;
            if (Status == null)
            {
                Status = unitdata.Status;
            }
            else
            {
                (Status as MyUnitStatus).StatusInit();
            }
            RankType = unitdata.RankType;
            AtkType = unitdata.AtkType;
            AbilityType = unitdata.AbilityType;
        }
    }
    public override void Init(int primaryKey, Sprite sprite)
    {
        MyUnit unitdata = Managers.Player.Inventory.GetItem<MyUnit>(primaryKey);
        if (unitdata == null)
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
        else
        {
            base.Init(primaryKey, sprite);
            Name = unitdata.Name;
            Description = unitdata.Description;
            if (Status == null)
            {
                Status = unitdata.Status;
            }
            else
            {
                (Status as MyUnitStatus).InvenStatus();
            }
            RankType = unitdata.RankType;
            AtkType = unitdata.AtkType;
            AbilityType = unitdata.AbilityType;
        }
    }

    public void AddHealth(float amount) => GetUpCasting<MyUnitStatus>().Health.AddValue(amount);
}
