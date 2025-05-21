using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestConditionEvaluator
{
    bool IsMatch(QuestData quest, int targetID);
    void ApplyProgress(QuestData quest, int amount);
}
