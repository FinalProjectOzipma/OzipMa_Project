using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class Tower : UserObject, IGettable
{
    public TowerStatus TowerStatus { get; private set; }
    public TowerAtkType AtkType { get; private set; }

    public List<TowerType> TowerTypes = new();
    public int Key { get; private set; }
    public List<DefaultTable.TowerAbilityDefaultValue> Abilities { get; private set; }

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

        // 정적 정보 채우기
        AtkType = towerData.AttackType;
        foreach(int t in towerData.TowerType)
        {
            TowerTypes.Add((TowerType)t);
        }
        Key = primaryKey;
        Abilities = Util.TableConverter<DefaultTable.TowerAbilityDefaultValue>(Managers.Data.Datas[Sheet.TowerAbilityDefaultValue]);
    }

    public void GradeUpdate()
    {
        // TODO:: 알아서하는걸로
        // ps.손나박한나
    }

    public void LevelUpdate()
    {
        // TODO:: 알아서하는걸로
        // ps.손나박한나
    }
}
