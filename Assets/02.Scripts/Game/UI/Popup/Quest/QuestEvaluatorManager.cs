using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvaluatorManager
{
    private Dictionary<ConditionType, IQuestConditionEvaluator> _evaluators;

    public QuestEvaluatorManager()
    {
        _evaluators = new()
        {
            { ConditionType.EnemyKill, new EnemyKillEvaluator() },
            { ConditionType.Connection, new ConnectionEvaluator() },
            { ConditionType.Reach, new ReachEvaluator()},
            { ConditionType.MyUnitCollect, new MyUnitCollecEvaluator() },
            { ConditionType.TowerCollect, new TowerCollectEvaluator()},
            { ConditionType.BossKill, new BossKillEvaluator()},
            { ConditionType.StageClear, new StageClearEvaluator()},
            { ConditionType.WaveClear, new WaveClearEvaluator()}
        };
    }

    public void Evaluate(QuestData quest, int targetID, int amount)
    {
        if (!_evaluators.TryGetValue(quest.ConditionType, out var evaluator))
            return;

        if (!evaluator.IsMatch(quest, targetID))
            return;

        evaluator.ApplyProgress(quest, amount);
    }
}
