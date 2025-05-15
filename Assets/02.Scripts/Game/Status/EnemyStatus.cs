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
        int index = Mathf.Min(Managers.Player.CurrentStage, stage.Count - 1);
        float modifierRatio = stage[index].AttackRatio;

        Attack.SetValue(row.Attack * modifierRatio);
        Attack.SetValueMultiples(1);
        AttackCoolDown.SetValue(row.AttackCoolDown);
        AttackRange.SetValue(row.AttackRange);

        Health.MaxValue = row.Health * modifierRatio;
        Health.SetValue(row.Health * modifierRatio);

        Defence.SetValue(row.Defence);
        MoveSpeed.SetValue(row.MoveSpeed);
    }
}
