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
        Description = towerData.Description;
        AtkType = towerData.AttackType;
        TowerType = towerData.AbilityType;
        RankType = towerData.Rank;

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
}
