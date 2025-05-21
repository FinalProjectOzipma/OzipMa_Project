using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKillEvaluator : IQuestConditionEvaluator
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
        if (quest.ConditionType != ConditionType.BossKill)
            return false;

        // -1이면 아무 대상이나 잡아도 됨
        return quest.TargetID == -1 || quest.TargetID == targetID;
    }
}
