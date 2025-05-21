using System;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using System.Threading.Tasks;


public class QuestManager
{
    public Dictionary<QuestType, List<QuestData>> QuestDatas;
    private Dictionary<ConditionType, List<QuestData>> conditionQuestIndex;
    private QuestEvaluatorManager evaluatorManager;

    private List<DefaultTable.QuestDataList> questData;

    private DateTime lastResetTime;

    public void Intialize()
    {
        questData = Util.TableConverter<DefaultTable.QuestDataList>(Managers.Data.Datas[Enums.Sheet.QuestDataList]);

        QuestDatas = new();
        conditionQuestIndex = new();
        evaluatorManager = new();

        for (int i = 0; i < questData.Count; i++)
        {
            switch (questData[i].QuestType)
            {
                case QuestType.Daily:
                    SetQuestType(QuestType.Daily, questData[i]);
                    break;
                case QuestType.Achivement:
                    SetQuestType(QuestType.Achivement, questData[i]);
                    break;
                case QuestType.Repeat:
                    SetQuestType(QuestType.Repeat, questData[i]);
                    break;
            }
        }
    }

    private void SetQuestType(QuestType type, DefaultTable.QuestDataList questData)
    {
        if (!QuestDatas.ContainsKey(type))
            QuestDatas[type] = new List<QuestData>();

        QuestData newQuest = new QuestData(
            questData.ID,
            questData.QuestType,
            questData.Name,
            questData.Description,
            questData.ConditionType,
            questData.TargetID,
            questData.Goal,
            questData.RewardGem
        );

        QuestDatas[type].Add(newQuest);

        if (!conditionQuestIndex.ContainsKey(newQuest.ConditionType))
            conditionQuestIndex[newQuest.ConditionType] = new List<QuestData>();

        conditionQuestIndex[newQuest.ConditionType].Add(newQuest);

    }

    public void RebuildConditionQuestIndex()
    {
        conditionQuestIndex.Clear();

        foreach (var questList in QuestDatas.Values)
        {
            foreach (var quest in questList)
            {
                if (!conditionQuestIndex.ContainsKey(quest.ConditionType))
                    conditionQuestIndex[quest.ConditionType] = new List<QuestData>();

                conditionQuestIndex[quest.ConditionType].Add(quest);
            }
        }
    }


    public List<QuestData> GetQuestList(QuestType questType)
    {
        if (QuestDatas.TryGetValue(questType, out var list))
            return list;

        return new List<QuestData>(); // 또는 null 처리
    }

    public Dictionary<string, QuestData> ConvertToFlatDictionary()
    {
        Dictionary<string, QuestData> result = new();

        foreach (var kvp in QuestDatas)
        {
            foreach (var quest in kvp.Value)
            {
                // 기본 키는 ID를 문자열로. 커스터마이징 가능.
                result[quest.Name.ToString()] = quest;
            }
        }

        return result;
    }

    public void UpdateQuestProgress(ConditionType condition, int targetID, int amount)
    {
        if (!conditionQuestIndex.TryGetValue(condition, out var questList))
            return;

        foreach (var quest in questList)
        {
            if (quest.State != QuestState.Doing) continue;

            evaluatorManager.Evaluate(quest, targetID, amount);
        }

    }

    public void SetDailyQuestZero()
    {
        for(int i = 0; i < QuestDatas[QuestType.Daily].Count; i++)
        {
            if(QuestDatas.TryGetValue(QuestType.Daily, out var dailyQuest))
            {
                dailyQuest[i].Progress = 0;
                dailyQuest[i].State = QuestState.Doing;
            }
        }
    }

    public void CheckAndResetIfNeeded()
    {
        DateTime currentUtc = Managers.Game.ServerUtcNow;

        DateTime todayMidnightUtc = new DateTime(
            currentUtc.Year,
            currentUtc.Month,
            currentUtc.Day,
            0, 0, 0,
            DateTimeKind.Utc);

        if (string.IsNullOrEmpty(Managers.Player.LastRestQuestTime))
        {
            lastResetTime = DateTime.MinValue;
        }
        else
        {
            lastResetTime = DateTime.Parse(Managers.Player.LastRestQuestTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }

        if (lastResetTime < todayMidnightUtc && currentUtc >= todayMidnightUtc)
        {
            SetDailyQuestZero();
            Managers.Player.LastRestQuestTime = currentUtc.ToString("o");
        }

    }


}
 