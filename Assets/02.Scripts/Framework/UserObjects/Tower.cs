using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class Tower : UserObject, IGettable
{
    public TowerStatus TowerStatus { get; set; }
    public AtkType AtkType { get; private set; }

    public AbilityType TowerType = new();
    public int Key { get; private set; }
    public static Dictionary<AbilityType, DefaultTable.AbilityDefaultValue> Abilities { get; private set; } // 속성 정보들 캐싱, 추후 다른 곳으로 빼고싶음.

    public T GetClassAddress<T>() where T : UserObject
    {
        return this as T;
    }

    public override void Init(int primaryKey, Sprite sprite)
    {
        base.Init(primaryKey, sprite);
        DefaultTable.Tower towerData = Managers.Data.Datas[Enums.Sheet.Tower][primaryKey] as DefaultTable.Tower;

        // 동적 정보 채우기
        TowerStatus = new TowerStatus(primaryKey);
        Status = TowerStatus;

        // 정적 정보 채우기
        Key = primaryKey;
        Name = towerData.Name;
        AtkType = towerData.AttackType;
        TowerType = towerData.AbilityType;

        if (Abilities == null)
        {
            Abilities = new();
            var abilities = Util.TableConverter<DefaultTable.AbilityDefaultValue>(Managers.Data.Datas[Sheet.AbilityDefaultValue]);
            foreach (var ability in abilities)
            {
                Abilities.Add(ability.AbilityType, ability);
            }
        }
    }

    /// <summary>
    /// 타워 진화
    /// </summary>
    /// <returns>진화 성공 여부</returns>
    public bool GradeUpdate()
    {
        if (TowerStatus.Grade.GetValue() < 5)
        {
            TowerStatus.MaxLevel.AddValue(1);
            TowerStatus.Attack.AddMultiples(0.5f);
            TowerStatus.AttackCoolDown.AddMultiples(-0.05f);
            TowerStatus.AttackRange.SetValue(TowerStatus.AttackRange.GetValue() * 1.02f);
            return true;
        }
        else
        {
            // TODO :: 풀각인데 또 뜨면 어떻게해줄건지?
        }
        return false;
    }

    /// <summary>
    /// 타워 강화
    /// </summary>
    /// <returns>강화 성공 여부</returns>
    public bool LevelUpdate()
    {
        if(TowerStatus.Level.GetValue() < TowerStatus.MaxLevel.GetValue())
        {
            TowerStatus.Level.AddValue(1);
            TowerStatus.Attack.AddMultiples(0.1f);
            //TowerStatus.AttackCoolDown.AddMultiples(-0.05f);
            //TowerStatus.AttackRange.SetValue(TowerStatus.AttackRange.GetValue() * 1.02f);
            return true;
        }
        return false;
    }
}
