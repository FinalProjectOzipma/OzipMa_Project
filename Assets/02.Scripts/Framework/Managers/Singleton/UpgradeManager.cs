using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using Firebase.Database;

public class UpgradeManager
{
    public int LevelUPGold;
    private float UpdateValue;

    private int TotalUpgradeGold;


    public event Action<int> OnChanagedUpgrade;


    public void Intialize()
    {
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

        if (Managers.Player.Gold >= LevelUPGold)
        {
            myUpgradeStatus.Level.AddValue(1);
            myUpgradeStatus.Attack.AddValue(5);
            myUpgradeStatus.Defence.AddValue(5);
            myUpgradeStatus.Health.AddValue(5);
            myUpgradeStatus.MoveSpeed.AddValue(5);

            Managers.Player.SpenGold(LevelUPGold);
            //myUnit.AttackCoolDown.AddMultiples(-myUnitUpdateValue);
            //myUnit.AttackRange.AddMultiples(myUnitUpdateValue);
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
            tower.TowerStatus.Attack.AddValue(5);
            tower.TowerStatus.AttackRange.AddValue(0.1f);
            

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



}
