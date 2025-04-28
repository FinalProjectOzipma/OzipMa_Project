using GoogleSheet;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : StatusBase
{
    public EntityHealth Health { get; set; } = new();
    

    public float MaxHealth { get; set; } = new();

    public FloatBase Defence { get; set; } = new();
    public FloatBase MoveSpeed { get; set; } = new();

    public EnemyStatus(DefaultTable.Enemy row)
    {
        Init();

        Attack.SetValue(row.Attack);
        AttackCoolDown.SetValue(row.AttackCoolDown);
        AttackRange.SetValue(row.AttackRange);

        Health.SetValue(row.Health);
        MaxHealth = Health.GetValue();

        Defence.SetValue(row.Defence);
        MoveSpeed.SetValue(row.MoveSpeed);
    }

    public void AddHealth(float amount) => Health.AddValue(amount);
}
