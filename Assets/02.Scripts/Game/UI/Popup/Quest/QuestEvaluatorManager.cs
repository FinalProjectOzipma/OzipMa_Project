using System.Collections.Generic;

public class QuestEvaluatorManager
{
    private Dictionary<ConditionType, IQuestConditionEvaluator> _evaluators;

    public QuestEvaluatorManager()
    {
        _evaluators = new()
        {
            { ConditionType.Connection, new ConnectionEvaluator() },
            { ConditionType.EnemyKill, new EnemyKillEvaluator() },
            { ConditionType.BossKill, new BossKillEvaluator()},
            { ConditionType.MyUnitCollect, new MyUnitCollecEvaluator() },
            { ConditionType.TowerCollect, new TowerCollectEvaluator()},
            { ConditionType.Reach, new ReachEvaluator()},
            { ConditionType.StageClear, new StageClearEvaluator()},
            { ConditionType.WaveClear, new WaveClearEvaluator()},
            { ConditionType.MyUnitInchen, new MyUnitInchenEvaluator()},
            { ConditionType.TowerInchen, new TowerInchenEvaluator()}
        };
    }

    public void Evaluate(QuestData quest, int targetID, int amount)
    {

        if (!_evaluators.TryGetValue(quest.ConditionType, out var evaluator))
            return;

        if (!evaluator.IsActive(quest))
            return;

        if (!evaluator.IsMatch(quest, targetID))
            return;

        evaluator.ApplyProgress(quest, amount);
    }
}
