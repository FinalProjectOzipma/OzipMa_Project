using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using Firebase.Database;
using Unity.VisualScripting;
using DefaultTable;

public class UpgradeManager
{
    public int LevelUPGold;
    private float UpdateValue;

    private int TotalUpgradeGold;


    public event Action<int> OnChanagedUpgrade;

    public List<DefaultTable.InchentMultiplier> inchentMultiplier;


    public void Intialize()
    {
        inchentMultiplier = Util.TableConverter<DefaultTable.InchentMultiplier>(Managers.Data.Datas[Enums.Sheet.InchentMultiplier]);
        LevelUPGold = 1000;
        UpdateValue = 0.1f;
        TotalUpgradeGold = 0;
    }

    public void LevelUpMyUnit(MyUnit myUnit)
    {
        MyUnitStatus myUpgradeStatus = myUnit.Status as MyUnitStatus;

        if (myUnit.Status.Level.GetValue() == myUnit.Status.MaxLevel.GetValue())
        {
            return;
        }

        if(myUnit.Status.Stack.GetValue() != myUnit.Status.MaxStack.GetValue())
        {
            return;
        }

        if(myUnit.Status.Grade.GetValue() == myUnit.Status.MaxStack.GetValue())
        {
            return;
        }

        if (Managers.Player.Gold >= LevelUPGold)
        {
            myUpgradeStatus.Level.AddValue(1);

            if (myUnit.Status.Level.GetValue() == myUnit.Status.MaxLevel.GetValue())
            {
                myUpgradeStatus.Level.SetValue(1);
                myUpgradeStatus.Grade.AddValue(1);
                ApplyInchentMyUnit(myUpgradeStatus);
                ApplyGradeMutipleMyUnit(myUpgradeStatus);
            }
            else
            {
                ApplyInchentMyUnit(myUpgradeStatus);
            }

            Managers.Player.SpenGold(LevelUPGold);
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
        if (tower.TowerStatus.Level.GetValue() == tower.TowerStatus.MaxLevel.GetValue())
        {
            Util.Log("만렙입니다");
            return;
        }

        if (Managers.Player.Gold >= LevelUPGold)
        {
            tower.TowerStatus.Level.AddValue(1);

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

            Managers.Player.SpenGold(LevelUPGold);
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
        switch(userObject.Level.GetValue())
        {
            case 1:
                userObject.Attack.ValueMultiples = 1.0f;
                userObject.Defence.ValueMultiples = 1.0f;
                userObject.Health.ValueMultiples = 1.0f;
                userObject.MoveSpeed.ValueMultiples = 1.0f;
                userObject.AttackCoolDown.ValueMultiples = 1.0f;
                break;
                break;
            case 2:
                userObject.Attack.AddMultiples(0.1f);
                userObject.Defence.AddMultiples(0.1f);
                userObject.Health.AddMultiples(0.1f);
                userObject.MoveSpeed.AddMultiples(0.1f);
                userObject.AttackCoolDown.AddMultiples(-0.1f);
                break;
            case 3:
                userObject.Attack.AddMultiples(0.13f);
                userObject.Defence.AddMultiples(0.13f);
                userObject.Health.AddMultiples(0.13f);
                userObject.MoveSpeed.AddMultiples(0.13f);
                userObject.AttackCoolDown.AddMultiples(-0.13f);
                break;
            case 4:
                userObject.Attack.AddMultiples(0.17f);
                userObject.Defence.AddMultiples(0.17f);
                userObject.Health.AddMultiples(0.17f);
                userObject.MoveSpeed.AddMultiples(0.17f);
                userObject.AttackCoolDown.AddMultiples(-0.17f);
                break;
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                userObject.Attack.AddMultiples(0.2f);
                userObject.Defence.AddMultiples(0.2f);
                userObject.Health.AddMultiples(0.2f);
                userObject.MoveSpeed.AddMultiples(0.2f);
                userObject.AttackCoolDown.AddMultiples(-0.2f);
                break;
            default:
                break;
        }
    }

    public void ApplyInchentTower(Tower userObject)
    {
        switch (userObject.Status.Level.GetValue())
        {
            case 1:
                userObject.TowerStatus.Attack.ValueMultiples = 1.0f;
                userObject.TowerStatus.AttackCoolDown.ValueMultiples = 1.0f;
                break;
            case 2:
                userObject.TowerStatus.Attack.AddMultiples(0.1f);
                userObject.TowerStatus.AttackCoolDown.AddMultiples(0.1f);
                break;
            case 3:
                userObject.TowerStatus.Attack.AddMultiples(0.13f);
                userObject.TowerStatus.AttackCoolDown.AddMultiples(0.13f);
                break;
            case 4:
                userObject.TowerStatus.Attack.AddMultiples(0.17f);
                userObject.TowerStatus.AttackCoolDown.AddMultiples(0.17f);
                break;
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                userObject.TowerStatus.Attack.AddMultiples(0.2f);
                userObject.TowerStatus.AttackCoolDown.AddMultiples(0.2f);
                break;
            default:
                break;
        }
    }
    public void ApplyGradeMutipleMyUnit(MyUnitStatus userObject)
    {
        int grade = userObject.Grade.GetValue();

        // 인덱스가 배열 범위 초과하지 않도록 예외 처리
        int index = Mathf.Clamp(grade - 1, 0, inchentMultiplier.Count - 1);

        var multiplier = inchentMultiplier[index];

        userObject.Attack.ValueMultiples = multiplier.AttackMultiplier;
        userObject.Defence.ValueMultiples = multiplier.DefMultiplier;
        userObject.Health.ValueMultiples = multiplier.HPMultiplier;
        userObject.AttackCoolDown.ValueMultiples = multiplier.CoolDownMultiplier;
    }

    public void ApplyGradeMutipleTower(Tower userObject)
    {
        int grade = userObject.TowerStatus.Grade.GetValue();

        // 인덱스가 배열 범위 초과하지 않도록 예외 처리
        int index = Mathf.Clamp(grade - 1, 0, inchentMultiplier.Count - 1);

        var multiplier = inchentMultiplier[index];

        userObject.TowerStatus.Attack.ValueMultiples = multiplier.AttackMultiplier;
        userObject.TowerStatus.AttackCoolDown.ValueMultiples = multiplier.CoolDownMultiplier;
    }



}
