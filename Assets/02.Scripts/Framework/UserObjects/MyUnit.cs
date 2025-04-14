using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnit : UserObject, IGettable
{
    public MyUnitStatus Status { get; set; }

    public T GetClassAddress<T>() where T : UserObject
    {
        return this as T;
    }

    public override void Init(int maxStack)
    {
        base.Init(maxStack);
        Status.Health.SetValue(Status.MaxHealth);
    }

    public void AddHealth(float amount) => Status.Health.AddValue(amount);
    
    public void GradeUpdate()
    {
        if (Grade.Value >= MaxGrade.Value)
        {
            Debug.Log("Grade is already Over!");
            return;
        }

        if (Stack.Value > MaxStack.Value)
        {
            Grade.AddValue(1);
            MaxGrade.AddValue(10);
            // TODO : 능력치를 올리는부분 & UI 업데이트하는부분추가해야함
        }
    }



    public void UpgradeLevel(int count = 1)
    {
        //레벨이 초과되는가
        //if (Level.Value >= MaxLevel.Value)
        //{
        //    Debug.Log("Level is already Over!");
        //    return;
        //}

        int singleUpgradeCost = Grade.Value /* * (int)RankType*/;
        int totalCost = singleUpgradeCost * count;

        //할인가 계산
        if (count >= 10)
        {
            totalCost -= (count / 10) * singleUpgradeCost;
        }
        if (Managers.Game.Money < totalCost)
        {
            Debug.Log("There is no Money.");
        }
        LevelUpdate(count);
    }

    public void LevelUpdate(int value = 1)
    {
        //Level.AddStat(value);
        // TODO : 능력치를 올리는부분 & UI 업데이트하는 부분 추가해야함
    }
}
