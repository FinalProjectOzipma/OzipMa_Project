using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestConditionEvaluator
{
    bool IsActive(QuestData quest);
    bool IsMatch(QuestData quest, int targetID);
    void ApplyProgress(QuestData quest, int amount);
}
