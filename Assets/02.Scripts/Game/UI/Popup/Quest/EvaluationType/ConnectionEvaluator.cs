public class ConnectionEvaluator : IQuestConditionEvaluator
{
    public void ApplyProgress(QuestData quest, int amount)
    {
        quest.Progress = quest.Goal;
        Util.Log("첫접속 성공" + quest.Progress.ToString());
        quest.State = QuestState.Done;
        quest.OnStateChanged?.Invoke(quest.State);
    }

    public bool IsActive(QuestData quest)
    {
        return quest.IsActive == 1;
    }

    public bool IsMatch(QuestData quest, int targetID)
    {
        return quest.ConditionType == ConditionType.Connection;
    }
}
