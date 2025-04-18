using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : StatusBase
{
    public EntityHealth Health { get; set; } = new();
    public float MaxHealth { get; set; } = new();

    public List<FloatBase> Defences { get; set; } = new();
    public FloatBase MoveSpeed { get; set; } = new();
    public AtkType AtkType { get; set; } = new();

    public EnemyStatus(DefaultTable.Enemy row)
    {
        Health.SetValue(row.Health);
        MaxHealth = Health.GetValue();
        for(int i = 0; i<row.Defence.Count; i++)
        {
            Defences.Add(new FloatBase());
            Defences[i].SetValue(row.Defence[i]);
        }

        MoveSpeed.SetValue(row.MoveSpeed);
    }
}
