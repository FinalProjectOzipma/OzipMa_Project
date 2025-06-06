using System;
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
        MyUnit unitdata = Managers.Player.Inventory.GetItem<MyUnit>(primaryKey);
        // 인벤토리에 기존에 없을 때
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
        //인벤토리에 있는 유닛일 경우
        else
        {
            var result = Util.TableConverter<DefaultTable.MyUnit>(Managers.Data.Datas[Enums.Sheet.MyUnit]);
            base.Init(primaryKey, sprite);
            Name = unitdata.Name;
            Description = unitdata.Description;

            if (Status == null)
            {
                Status = new MyUnitStatus(primaryKey);
            }

            (Status as MyUnitStatus).InvenStatus();

            RankType = unitdata.RankType;
            AtkType = unitdata.AtkType;
            AbilityType = unitdata.AbilityType;
        }
        
        Managers.Analytics.AnalyticsUnitSummoned(primaryKey.ToString(), Name, Enum.GetName(typeof(AbilityType), AtkType), Status.Level.Value, "auto", Managers.Player.CurrentWave);
    }

    public void AddHealth(float amount) => GetUpCasting<MyUnitStatus>().Health.AddValue(amount);
}
