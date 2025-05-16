using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : StatusBase
{
    private List<DefaultTable.Stage> stage;

    public EnemyStatus(DefaultTable.Enemy row)
    {
        stage = Util.TableConverter<DefaultTable.Stage>(Managers.Data.Datas[Enums.Sheet.Stage]);
        Init(row);
    }

    public void Init(DefaultTable.Enemy row)
    {
        PlayerManager playerManager = Managers.Player;
        Attack.SetValue(row.Attack * stage[playerManager.CurrentKey].AttackRatio);
        Attack.SetValueMultiples(1);
        AttackCoolDown.SetValue(row.AttackCoolDown);
        AttackRange.SetValue(row.AttackRange);

        Health.MaxValue = row.Health * stage[playerManager.CurrentKey].HealthRatio;
        Health.SetValue(row.Health * stage[playerManager.CurrentKey].HealthRatio);

        Defence.SetValue(row.Defence * stage[playerManager.CurrentKey].DefenceRatio);
        MoveSpeed.SetValue(row.MoveSpeed);
    }
}
