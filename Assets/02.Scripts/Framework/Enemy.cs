using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public int Reward;
    public bool IsBoss;

    public int PrimaryKey { get; private set; }
    public Sprite Sprite { get; private set; }
    public EnemyStatus Status { get; set; }
    public AtkType AtkType { get; set; }
    

    public Enemy(int primaryKey, Sprite sprite)
    {
        PrimaryKey = primaryKey;
        Sprite = sprite;

        Init(primaryKey, sprite);
    }

    public void Init(int primaryKey, Sprite sprite)
    {
        var result = Util.TableConverter<DefaultTable.Enemy>(Managers.Data.Datas[Enums.Sheet.Enemy]);
        Status = new EnemyStatus(result[primaryKey]);
        AtkType = result[primaryKey].AttackType;
    }

    public void AddHealth(float amount) => Status.Health.AddValue(amount);
}
