using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager
{
    public int LevelUPGold;

    public int TotalUpgradeGold;


    public event Action<int> OnChanagedUpgrade;

    public List<DefaultTable.InchentMultiplier> InchentMultiplier;
    public List<DefaultTable.LevelUpValue> LevelUpValues;
    public List<DefaultTable.Research> ResearchesUpgradeTable;


    public void Intialize()
    {
        InchentMultiplier = Util.TableConverter<DefaultTable.InchentMultiplier>(Managers.Data.Datas[Enums.Sheet.InchentMultiplier]);
        LevelUpValues = Util.TableConverter<DefaultTable.LevelUpValue>(Managers.Data.Datas[Enums.Sheet.LevelUpValue]);
        ResearchesUpgradeTable = Util.TableConverter<DefaultTable.Research>(Managers.Data.Datas[Enums.Sheet.Research]);
        LevelUPGold = 1000;
        TotalUpgradeGold = 0;
    }

    public void LevelUpMyUnit(MyUnit myUnit)
    {
        MyUnitStatus myUpgradeStatus = myUnit.Status as MyUnitStatus;
        int gold = GetLevelUpGold(myUnit);
        if (Managers.Player.Gold >= TotalUpgradeGold)
        {
            myUpgradeStatus.Level.AddValue(1);
            myUpgradeStatus.Stack.AddValue(-myUpgradeStatus.MaxStack.GetValue());
            IncreaseRequireCard(myUnit);

            if (myUnit.Status.Level.GetValue() == myUnit.Status.MaxLevel.GetValue())
            {
                myUpgradeStatus.Level.SetValue(1);
                myUpgradeStatus.Grade.AddValue(1);
                ApplyInchentMyUnit(myUpgradeStatus);
                //ApplyGetValue(myUpgradeStatus);
                ApplyGradeMutipleMyUnit(myUpgradeStatus);
            }
            else
            {
                ApplyInchentMyUnit(myUpgradeStatus);
                //ApplyGetValue(myUpgradeStatus);
            }

            Managers.Player.SpenGold(gold);
            return;
        }
        else
        {
            // 방어용
            Util.Log("왜 돈이 부족하니?");
            return;
        }
    }



    public void LevelUpTower(Tower tower)
    {

        if (Managers.Player.Gold >= TotalUpgradeGold)
        {
            int gold = GetLevelUpGold(tower);
            tower.TowerStatus.Level.AddValue(1);
            tower.TowerStatus.Stack.AddValue(-tower.TowerStatus.MaxStack.GetValue());
            IncreaseRequireCard(tower);

            if (tower.TowerStatus.Level.GetValue() == tower.TowerStatus.MaxLevel.GetValue())
            {
                tower.TowerStatus.Level.SetValue(1);
                tower.TowerStatus.Grade.AddValue(1);
                ApplyInchentTower(tower);
                ApplyGradeMutipleTower(tower);
            }
            else
            {
                ApplyInchentTower(tower);
            }

            Managers.Player.SpenGold(gold);
            return;
        }
        else
        {
            // 방어용
            Util.Log("왜 돈이 부족하니?");
            return;
        }
    }

    public int GetUpgradeGold() => TotalUpgradeGold;


    public void OnUpgradeGold(int gold)
    {
        TotalUpgradeGold += gold;
        OnChanagedUpgrade?.Invoke(TotalUpgradeGold);
    }


    public void RefresgUpgradeGold()
    {
        TotalUpgradeGold = 0;
        OnChanagedUpgrade?.Invoke(TotalUpgradeGold);
    }


    public void ApplyInchentMyUnit(MyUnitStatus userObject)
    {
        int level = userObject.Level.GetValue();

        // 레벨 1은 배율 초기화
        if (level == 1)
        {
            userObject.Attack.SetValueMultiples(1.0f);
            userObject.Defence.SetValueMultiples(1.0f);
            userObject.Health.SetValueMultiples(1.0f);
            userObject.MoveSpeed.SetValueMultiples(1.0f);
            userObject.AttackCoolDown.SetValueMultiples(1.0f);
            return;
        }

        // 레벨업 수치 인덱스: Lv.2 → index 0, Lv.3 → index 1, ...
        int index = level - 2;
        if (index < 0 || index >= LevelUpValues.Count)
            return;

        float statUp = LevelUpValues[index].StatUP;

        // 공통 스탯 적용
        userObject.Attack.AddMultiples(statUp);
        userObject.Defence.AddMultiples(statUp);
        userObject.Health.AddMultiples(statUp);
        userObject.MoveSpeed.AddMultiples(statUp);

        // 쿨타임은 반대로 감소
        userObject.AttackCoolDown.AddMultiples(-statUp);

    }


    public void ApplyInchentTower(Tower userObject)
    {

        int level = userObject.TowerStatus.Level.GetValue();

        // 레벨 1은 배율 초기화
        if (level == 1)
        {
            userObject.TowerStatus.Attack.SetValueMultiples(1.0f);
            userObject.TowerStatus.AttackCoolDown.SetValueMultiples(1.0f);
            return;
        }

        // 레벨업 수치 인덱스: Lv.2 → index 0, Lv.3 → index 1, ...
        int index = level - 2;
        if (index < 0 || index >= LevelUpValues.Count)
            return;

        float statUp = LevelUpValues[index].StatUP;

        userObject.TowerStatus.Attack.AddMultiples(statUp);
        userObject.TowerStatus.AttackCoolDown.AddMultiples(-statUp);
    }

    public void ApplyGetValue(MyUnitStatus userObject)
    {
        userObject.Attack.Value = userObject.Attack.GetValue();
        userObject.Defence.Value = userObject.Defence.GetValue();
        userObject.InitHealth();
        userObject.Health.MaxValue = userObject.Health.GetValue();
        userObject.AttackCoolDown.Value = userObject.AttackCoolDown.GetValue();
    }

    public void ApplyGradeMutipleMyUnit(MyUnitStatus userObject)
    {
        int grade = userObject.Grade.GetValue();

        // 인덱스가 배열 범위 초과하지 않도록 예외 처리
        int index = Mathf.Clamp(grade - 1, 0, InchentMultiplier.Count - 1);

        var multiplier = InchentMultiplier[index];

        userObject.Attack.SetGradeMultiple(multiplier.AttackMultiplier);
        userObject.Defence.SetGradeMultiple(multiplier.DefMultiplier);
        userObject.Health.SetGradeMultiple(multiplier.HPMultiplier);
        userObject.AttackCoolDown.SetGradeMultiple(multiplier.CoolDownMultiplier);
    }


    public void ApplyGradeMutipleTower(Tower userObject)
    {
        int grade = userObject.TowerStatus.Grade.GetValue();

        // 인덱스가 배열 범위 초과하지 않도록 예외 처리
        int index = Mathf.Clamp(grade - 1, 0, InchentMultiplier.Count - 1);

        var multiplier = InchentMultiplier[index];

        userObject.TowerStatus.Attack.SetGradeMultiple(multiplier.AttackMultiplier);
        userObject.TowerStatus.AttackCoolDown.SetGradeMultiple(multiplier.CoolDownMultiplier);
    }

    public int GetLevelUpGold(UserObject userObject)
    {
        int level = userObject.Status.Level.GetValue();

        if (level < 0 || level >= 10)
            return 0;

        return LevelUpValues[level - 1].LevelUpGold;
    }

    public void IncreaseRequireCard(UserObject userObject)
    {
        int level = userObject.Status.Level.GetValue();

        // 레벨 1은 배율 초기화
        if (level == 1)
        {
            userObject.Status.Stack.SetValue(10);
            return;
        }

        // 레벨업 수치 인덱스: Lv.2 → index 0, Lv.3 → index 1, ...
        int index = level - 2;
        if (index < 0 || index >= LevelUpValues.Count)
            return;

        int requirCard = LevelUpValues[index].RequireCard;

        userObject.Status.MaxStack.SetValue(requirCard);

    }

    public float GetResearchValue(ResearchUpgradeType type, int i)
    {
        switch (type)
        {
            case ResearchUpgradeType.Attack:
                return ResearchesUpgradeTable[i - 1].Attack;
            case ResearchUpgradeType.Defence:
                return ResearchesUpgradeTable[i - 1].Defence;
            case ResearchUpgradeType.Core:
                Util.Log("체력 연구 성공");
                return ResearchesUpgradeTable[i - 1].CoreHealth;
            default:
                return 0.0f;
        }
    }


}
