using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitInchenEvaluator : IQuestConditionEvaluator
{
    public void ApplyProgress(QuestData quest, int amount)
    {
        quest.AddProgress(amount);

        if (quest.Type == QuestType.Repeat)
        {
            Util.Log("반복퀘 데이터 :" + quest.Progress);
            Util.Log("반복퀘 설명 :" + quest.Description);
        }
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
        return quest.ConditionType == ConditionType.MyUnitInchen &&
       (quest.TargetID == -1 || quest.TargetID == targetID);
    }
}
