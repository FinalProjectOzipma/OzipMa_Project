using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitCollecEvaluator : IQuestConditionEvaluator
{
    public void ApplyProgress(QuestData quest, int amount)
    {
        quest.AddProgress(amount);
        quest.CheckDone();

    }

    public bool IsActive(QuestData quest)
    {
        return quest.IsActive == 1;
    }

    public bool IsMatch(QuestData quest, int targetID)
    {
        if (quest.ConditionType != ConditionType.MyUnitCollect)
            return false;

        // -1이면 아무 대상 수집
        return quest.TargetID == -1 || quest.TargetID == targetID;

    }
}
