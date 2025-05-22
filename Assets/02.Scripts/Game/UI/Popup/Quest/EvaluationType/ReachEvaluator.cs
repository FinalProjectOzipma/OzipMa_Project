using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachEvaluator : IQuestConditionEvaluator
{
    public void ApplyProgress(QuestData quest, int amount)
    {
        quest.AddProgress(amount);
        if (quest.Progress >= quest.Goal)
        {
            quest.Progress = quest.Goal;
            quest.State = QuestState.Done;
            quest.OnStateChanged?.Invoke(quest.State);
        }
    }
    public bool IsActive(QuestData quest)
    {
        return quest.IsActive == 1;
    }

    public bool IsMatch(QuestData quest, int targetID)
    {
        return quest.ConditionType == ConditionType.Reach &&
       (quest.TargetID == -1 || quest.TargetID == targetID);
    }
}
