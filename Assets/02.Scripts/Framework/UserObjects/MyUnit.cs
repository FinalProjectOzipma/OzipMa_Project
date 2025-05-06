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
        Status = new MyUnitStatus(primaryKey, result);
        RankType = result[primaryKey].Rank;
        AtkType = result[primaryKey].AttackType;
        AbilityType = result[primaryKey].AbilityType;
    }

    public void AddHealth(float amount) => GetUpCasting<MyUnitStatus>().Health.AddValue(amount);
    
    public void GradeUpdate()
    {
        if (Status.Grade.Value >= MaxGrade.Value)
        {
            Debug.Log("Grade is already Over!");
            return;
        }

        if (Status.Stack.Value > Status.MaxStack.Value)
        {
            Status.Grade.AddValue(1);
            MaxGrade.AddValue(10);
            // TODO : 능력치를 올리는부분 & UI 업데이트하는부분추가해야함
        }
    }

    public void UpgradeLevel(int count = 1)
    {
        //레벨이 초과되는가
        if (Status.Level.Value >= Status.MaxLevel.Value)
        {
            Debug.Log("Level is already Over!");
            return;
        }

        int singleUpgradeCost = Status.Grade.Value * (int)RankType;
        int totalCost = singleUpgradeCost * count;

        //할인가 계산
        if (count >= 10)
        {
            totalCost -= (count / 10) * singleUpgradeCost;
        }
        if (Managers.Player.Money < totalCost)
        {
            Debug.Log("There is no Money.");
        }
        LevelUpdate(count);
    }// 손나박한나, 나만 믿어 ( 개발 리더 - 편상윤 - )

    public void LevelUpdate(int value = 1)
    {
        //Level.AddStat(value);
        // TODO : 능력치를 올리는부분 & UI 업데이트하는 부분 추가해야함
    }
}
