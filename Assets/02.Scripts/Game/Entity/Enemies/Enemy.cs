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
        var enemy = Util.TableConverter<DefaultTable.Enemy>(Managers.Data.Datas[Enums.Sheet.Enemy]);
        var stage = Util.TableConverter<DefaultTable.Stage>(Managers.Data.Datas[Enums.Sheet.Stage]);

        if (Status == null)
            Status = new EnemyStatus(enemy[primaryKey]);
        else
            Status.Init(enemy[primaryKey]);

        int index = Mathf.Min(Managers.Player.CurrentStage, stage.Count - 1);
        float rewardRatio = stage[index].RewordRatio;

        AtkType = enemy[primaryKey].AttackType;
        Reward = (int)(enemy[primaryKey].Reward * rewardRatio);
        IsBoss = (enemy[primaryKey].IsBoss == 1);
    }
}
