using DefaultTable;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager
{
    public int LevelUPGold;

    public int TotalUpgradeGold;


    public event Action<int> OnChanagedUpgrade;

    public List<DefaultTable.InchentMultiplier> EnchantMultiplier;
    public List<DefaultTable.LevelUpValue> LevelUpValues;
    public List<DefaultTable.Research> ResearchesUpgradeTable;


    public void Intialize()
    {
        EnchantMultiplier = Util.TableConverter<DefaultTable.InchentMultiplier>(Managers.Data.Datas[Enums.Sheet.InchentMultiplier]);
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
            Managers.Quest.UpdateQuestProgress(ConditionType.MyUnitInchen, -1, 1);
            myUpgradeStatus.Stack.AddValue(-myUpgradeStatus.MaxStack.GetValue());

            myUpgradeStatus.Level.AddValue(1);

            if (myUnit.Status.Level.GetValue() > myUnit.Status.MaxLevel.GetValue())
            {
                myUpgradeStatus.Grade.AddValue(1);

                if (myUpgradeStatus.Grade.GetValue() == myUnit.MaxGrade.GetValue())
                {
                    myUpgradeStatus.Level.SetValue(10);
                }
                else
                {
                    myUpgradeStatus.Level.SetValue(1);
                }

                ApplyEnchantMyUnit(myUpgradeStatus);
                ApplyGradeMutipleMyUnit(myUpgradeStatus);
            }
            else
            {
                ApplyEnchantMyUnit(myUpgradeStatus);

            }


            IncreaseRequireCard(myUnit);

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
            Managers.Quest.UpdateQuestProgress(ConditionType.TowerInchen, -1, 1);
            int gold = GetLevelUpGold(tower);
            tower.TowerStatus.Stack.AddValue(-tower.TowerStatus.MaxStack.GetValue());
            tower.TowerStatus.Level.AddValue(1);

            if (tower.TowerStatus.Level.GetValue() > tower.TowerStatus.MaxLevel.GetValue())
            {
                tower.TowerStatus.Grade.AddValue(1);

                if(tower.TowerStatus.Grade.GetValue() == tower.MaxGrade.GetValue())
                {
                    tower.TowerStatus.Level.SetValue(10);
                }
                else
                {
                    tower.TowerStatus.Level.SetValue(1);
                }

                ApplyEnchantTower(tower);
                ApplyGradeMutipleTower(tower);
            }
            else
            {
                ApplyEnchantTower(tower);
            }

            IncreaseRequireCard(tower);

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


    public void ApplyEnchantMyUnit(MyUnitStatus userObject)
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
        int index = level - 1;
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


    public void ApplyEnchantTower(Tower userObject)
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
        int index = level - 1;
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
        int index = Mathf.Clamp(grade - 1, 0, EnchantMultiplier.Count - 1);

        var multiplier = EnchantMultiplier[index];

        userObject.Attack.SetGradeMultiple(multiplier.AttackMultiplier);
        userObject.Defence.SetGradeMultiple(multiplier.DefMultiplier);
        userObject.Health.SetGradeMultiple(multiplier.HPMultiplier);
        userObject.AttackCoolDown.SetGradeMultiple(multiplier.CoolDownMultiplier);
    }


    public void ApplyGradeMutipleTower(Tower userObject)
    {
        int grade = userObject.TowerStatus.Grade.GetValue();

        // 인덱스가 배열 범위 초과하지 않도록 예외 처리
        int index = Mathf.Clamp(grade - 1, 0, EnchantMultiplier.Count - 1);

        var multiplier = EnchantMultiplier[index];

        userObject.TowerStatus.Attack.SetGradeMultiple(multiplier.AttackMultiplier);
        userObject.TowerStatus.AttackCoolDown.SetGradeMultiple(multiplier.CoolDownMultiplier);
    }

    public int GetLevelUpGold(UserObject userObject)
    {
        int level = userObject.Status.Level.GetValue();

        if (level < 0 || level > 10)
            return 0;

        return LevelUpValues[level - 1].LevelUpGold;
    }

    public void IncreaseRequireCard(UserObject userObject)
    {
        int level = userObject.Status.Level.GetValue();
        Util.Log("현재레벨 : " + level.ToString());

        // 레벨 1은 배율 초기화
        if (level == 1)
        {
            userObject.Status.MaxStack.SetValue(10);
            return;
        }

        // 레벨업 수치 인덱스: Lv.2 → index 0, Lv.3 → index 1, ...
        int index = level - 1;
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
                return ResearchesUpgradeTable[i - 1].CoreHealth;
            default:
                return 0.0f;
        }
    }


}
