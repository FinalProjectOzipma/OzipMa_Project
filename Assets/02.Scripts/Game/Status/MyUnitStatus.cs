using System.Collections.Generic;

public class MyUnitStatus : StatusBase
{
    public EntityHealth Health;
    public float MaxHealth;

    public FloatBase Defence;
    public FloatBase MoveSpeed;
    public Enums.UnitAtkType AtkType;

    public MyUnitStatus(Dictionary<string, object> Row)
    {
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
