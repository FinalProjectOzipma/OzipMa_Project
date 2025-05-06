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
        int index = Mathf.Min(Managers.Player.CurrentStage, stage.Count - 1);
        float modifierRatio = stage[index].ModifierRatio;

        Attack.SetValue(row.Attack * modifierRatio);
        Attack.SetValueMultiples(1);
        AttackCoolDown.SetValue(row.AttackCoolDown);
        AttackRange.SetValue(row.AttackRange);

        Health.SetMaxHealth(row.Health * modifierRatio);
        Health.SetValue(row.Health * modifierRatio);
        MaxHealth = Health.GetValue();


        Defence.SetValue(row.Defence);
        MoveSpeed.SetValue(row.MoveSpeed);
    }

    public void InitHealth() => Health.SetValue(MaxHealth);
    public void AddHealth(float amount, GameObject go) => Health.AddValue(amount);     
    
}
