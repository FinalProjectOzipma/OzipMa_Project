using GoogleSheet;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyStatus : StatusBase
{
    private List<DefaultTable.Stage> stage;
    public EntityHealth Health { get; set; } = new();
    

    public float MaxHealth { get; set; } = new();

    public FloatBase Defence { get; set; } = new();
    public FloatBase MoveSpeed { get; set; } = new();

    public EnemyStatus(DefaultTable.Enemy row)
    {
        stage = Util.TableConverter<DefaultTable.Stage>(Managers.Data.Datas[Enums.Sheet.Stage]);
        Init(row);
    }

    public void Init(DefaultTable.Enemy row)
    {
        int index = Managers.Player.CurrentStage % stage.Count;
        float ratio = stage[index].ModifierRatio;

        Attack.SetValue(row.Attack * ratio);
        AttackCoolDown.SetValue(row.AttackCoolDown);
        AttackRange.SetValue(row.AttackRange);

        Health.SetValue(row.Health * ratio);
        MaxHealth = Health.GetValue();

        Defence.SetValue(row.Defence);
        MoveSpeed.SetValue(row.MoveSpeed);
    }

    public void InitHealth() => Health.SetValue(MaxHealth);
    public void AddHealth(float amount, GameObject go) => Health.AddValue(amount);     
    
}
