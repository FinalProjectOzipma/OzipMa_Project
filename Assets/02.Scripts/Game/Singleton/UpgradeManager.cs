using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

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
            Util.Log("만렙입니다");
            return;
        }

        if (Managers.Player.gold >= LevelUPGold)
        {
            myUpgradeStatus.Level.AddValue(1);
            myUpgradeStatus.Attack.AddMultiples(UpdateValue);
            myUpgradeStatus.Defence.AddMultiples(UpdateValue);
            myUpgradeStatus.Health.AddMultiples(UpdateValue);
            myUpgradeStatus.MoveSpeed.AddMultiples(UpdateValue);

            Managers.Player.SpenGold(LevelUPGold);
            Util.Log("업그레이드 완료" + myUnit.Name + " " + myUnit.Status.Level.GetValueToString());
            Util.Log("MaxLevel" + myUnit.Status.MaxLevel.GetValueToString());
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

        if (Managers.Player.gold >= LevelUPGold)
        {
            tower.TowerStatus.Level.AddValue(1);
            tower.TowerStatus.Attack.AddMultiples(UpdateValue);

            Managers.Player.SpenGold(LevelUPGold);
            Util.Log("업그레이드 완료" + tower.Name + " " + tower.TowerStatus.Level);
            return;
        }
        else
        {
            // 방어용
            Util.Log("왜 돈이 부족하니?");
            return;
        }
    }



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
