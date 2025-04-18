using GoogleSheet;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyStatus : StatusBase
{
    public EntityHealth Health { get; set; } = new();
    public float MaxHealth { get; set; } = new();

    public List<FloatBase> Defences { get; set; } = new();
    public FloatBase MoveSpeed { get; set; } = new();
    public AtkType AtkType { get; set; } = new();

    public EnemyStatus(DefaultTable.Enemy row)
    {
        Init();

        Attack.SetValue(row.Attack);
        AttackCoolDown.SetValue(row.AttackCoolDown);
        AttackRange.SetValue(row.AttackRange);

        Health.SetValue(row.Health);
        MaxHealth = Health.GetValue();
        for (int i = 0; i < row.Defence.Count; i++)
        {
            Defences.Add(new FloatBase());
            Defences[i].SetValue(row.Defence[i]);
        }

        MoveSpeed.SetValue(row.MoveSpeed);
    }
}
