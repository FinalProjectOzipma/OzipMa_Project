using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearEvaluator : IQuestConditionEvaluator
{
    public void ApplyProgress(QuestData quest, int amount)
    {
        quest.AddProgress(amount);

        if (quest.Type == QuestType.Repeat)
        {
            Util.Log("반복퀘 데이터 :" + quest.Progress);
            Util.Log("반복퀘 설명 :" + quest.Description);
        }
        quest.CheckDone();
    }

    public bool IsActive(QuestData quest)
    {
        return quest.IsActive == 1;
    }

    public bool IsMatch(QuestData quest, int targetID)
    {
        return quest.ConditionType == ConditionType.StageClear &&
       (quest.TargetID == -1 || quest.TargetID == targetID);
    }
}
