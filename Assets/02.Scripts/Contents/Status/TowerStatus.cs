using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;

public class TowerStatus : StatusBase
{
    public Enums.TowerAtkType AtkType;
    public List<Enums.TowerType> TowerType;

    public TowerStatus(Dictionary<string, object> Row)
    {
        // TODO :: 연두님과 상의

        /*name = Row[“Name”];
        description = Row[“Descriptopn”];
        rankType = Row[“RankType”];

        attack.SetStat(Row[“Attack”]);
        TowerattackType = Row[“TowerAttackType”];
        attackCooldown.SetStat(Row[“AttackCooldown”]);
        attackRange.SetStat(Row[“AttackRange”]);
        level = 1;*/
    }
}
