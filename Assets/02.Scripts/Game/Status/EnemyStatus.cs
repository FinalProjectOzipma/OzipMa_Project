using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : StatusBase
{
    public int Key;
    public EntityHealth Health;
    public float MaxHealth;

    public List<FloatBase> Defences;
    public FloatBase MoveSpeed;
    public Enums.AtkType AtkType;
    public IntegerBase Reward;
    public bool IsBoss;

    public EnemyStatus(Dictionary<string, object> Row)
    {
        Health = new EntityHealth();
        MaxHealth = Health.GetValue();
        Defences = new List<FloatBase>();
        MoveSpeed = new FloatBase();
        AtkType = Enums.AtkType.None;
        Reward = new IntegerBase();
        IsBoss = false;

        // TODO:: 연두님과 진실의 방

        /*name = Row[“Name”];
        description = Row[“Descriptopn”];
        rankType = Row[“RankType”];

        health.SetStat(Row[“Health”]);
        maxHealth = health;

        attack.SetStat(Row[“Attack”]); 
        defence.SetStat(Row[“Defence”]);
        moveSpeed.SetStat(Row[“MoveSpeed”]);
        attackType = Row[“AttackType”];
        attackCooldown.SetStat(Row[“AttackCooldown”]);
        attackRange.SetStat(Row[“AttackRange”]);
        level = 1;*/
    }
}
