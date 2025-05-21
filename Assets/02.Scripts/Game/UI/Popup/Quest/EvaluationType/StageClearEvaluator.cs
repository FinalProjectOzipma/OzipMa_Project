using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearEvaluator : IQuestConditionEvaluator
{
    public void ApplyProgress(QuestData quest, int amount)
    {
        quest.Progress += amount;
        if (quest.Progress >= quest.Goal)
        {
            quest.Progress = quest.Goal;
            quest.State = QuestState.Done;
        }
    }

    public bool IsMatch(QuestData quest, int targetID)
    {
        return quest.ConditionType == ConditionType.StageClear &&
       (quest.TargetID == -1 || quest.TargetID == targetID);
    }
}
