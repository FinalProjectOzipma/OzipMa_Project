using System;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class QuestManager
{
    public Dictionary<QuestType, List<QuestData>> QuestDatas;
    private Dictionary<ConditionType, List<QuestData>> conditionQuestIndex;
    private QuestEvaluatorManager evaluatorManager;

    private List<DefaultTable.QuestDataList> questData;

    private DateTime lastResetTime;

    public Action OnAnyQuestCompleted;

    /// <summary>
    /// 저장 값 없으면 로드되는 초기값
    /// </summary>

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

    /// <summary>
    /// 서버에서 로드 퀘스트 데이터에 구독해제 되서 다시 구독해주는 메서드
    /// </summary>
    public void ResisterQuestDatas()
    {
        List<QuestData> DailyQuest = GetQuestList(QuestType.Daily);
        List<QuestData> AchivementQuest = GetQuestList(QuestType.Achivement);

        for (int i = 0; i < DailyQuest.Count; i++)
        {
            DailyQuest[i].OnStateChanged += HandleQuestCompleted;
        }

        for (int i = 0; i < AchivementQuest.Count; i++)
        {
            AchivementQuest[i].OnStateChanged += HandleQuestCompleted;
        }
    }

    /// <summary>
    /// 처음 접속 시 퀘스트 타입별로 딕셔너리로 초기값 묶어주는 메서드
    /// </summary>
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
            questData.RewardGem,
            questData.RewardGold,
            questData.IsActive
        );

        QuestDatas[type].Add(newQuest);

        newQuest.OnStateChanged += HandleQuestCompleted;

        if (!conditionQuestIndex.ContainsKey(newQuest.ConditionType))
            conditionQuestIndex[newQuest.ConditionType] = new List<QuestData>();

        conditionQuestIndex[newQuest.ConditionType].Add(newQuest);


    }


    public void ResetEtcQuest()
    {
        RebuildConditionQuestIndex();
        ResisterQuestDatas();
        ResetRepeatQuest();
    }


    /// <summary>
    /// 퀘스트 종류로 다시 분류해서 딕셔너리로 저장하는 메서드
    /// </summary>
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

    /// <summary>
    /// QuestDats에서 원하는 퀘스트 타입을 가져오는 메서드
    /// </summary>
    public List<QuestData> GetQuestList(QuestType questType)
    {
        if (QuestDatas.TryGetValue(questType, out var list))
            return list;

        return new List<QuestData>(); // 또는 null 처리
    }


    /// <summary>
    /// 서버에 저장하기 위해 Dictionary<string,QuestData> 으로 변환
    /// </summary>
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

    /// <summary>
    /// 실제 퀘스트 진행을 업데이트해주는 메서드
    /// </summary>
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


    /// <summary>
    /// Daily 퀘스트 초기화 진행률과 상태를 초기화 시켜주는 메서드
    /// </summary>
    public void SetDailyQuestZero()
    {
        for (int i = 0; i < QuestDatas[QuestType.Daily].Count; i++)
        {
            if (QuestDatas.TryGetValue(QuestType.Daily, out var dailyQuest))
            {
                dailyQuest[i].SetProgress(0);
                dailyQuest[i].State = QuestState.Doing;
            }
        }
    }

    /// <summary>
    /// 게임이 자정을 넘었는지 확인하고 퀘스트 진행률 초기화
    /// </summary>
    public void CheckAndResetIfNeeded()
    {
        //DateTime currentUtc = DateTime.UtcNow.AddHours(9);
        DateTime currentUtc = Managers.Game.ServerUtcNow.AddHours(9);

        DateTime todayMidnightUtc = new DateTime(
            currentUtc.Year,
            currentUtc.Month,
            currentUtc.Day,
            0, 0, 0,
            DateTimeKind.Unspecified);

        if (string.IsNullOrEmpty(Managers.Player.LastRestQuestTime))
        {
            lastResetTime = DateTime.MinValue;
        }
        else
        {
            lastResetTime = DateTime.Parse(Managers.Player.LastRestQuestTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }
        Util.Log($"Current UTC: {currentUtc} (Kind: {currentUtc.Kind})");
        Util.Log($"Today Midnight UTC: {todayMidnightUtc} (Kind: {todayMidnightUtc.Kind})");

        if (lastResetTime < todayMidnightUtc && currentUtc >= todayMidnightUtc)
        {
            SetDailyQuestZero();
            Managers.Player.LastRestQuestTime = currentUtc.ToString("o");
        }

    }

    /// <summary>
    /// 게임 종류 시 반복 퀘스트 진행률 및 상태 초기화
    /// </summary>
    public void ResetRepeatQuest()
    {
        List<QuestData> repeaQuest = GetQuestList(QuestType.Repeat);

        for (int i = 0; i < repeaQuest.Count; i++)
        {
            repeaQuest[i].Progress = 0;
            repeaQuest[i].State = QuestState.Doing;
            repeaQuest[i].IsActive = 0;
        }
    }


    /// <summary>
    /// QuestData에 구독해서 UI_Main에 진행 상황을 연결해주는 메서드
    /// </summary>
    private void HandleQuestCompleted(QuestState questState)
    {
        OnAnyQuestCompleted?.Invoke();
    }


    /// <summary>
    /// 데일리퀘스트와 업적퀘스트가 하나라도 완료되면 UI_Main에 알리는 메서드
    /// </summary>
    public bool HasAnyCompletedQuest()
    {
        List<QuestData> DailyQuest = GetQuestList(QuestType.Daily);
        List<QuestData> AchivementQuest = GetQuestList(QuestType.Achivement);

        for (int i = 0; i < DailyQuest.Count; i++)
        {
            if (DailyQuest[i].State == QuestState.Done)
            {
                return true;
            }
        }

        for (int i = 0; i < AchivementQuest.Count; i++)
        {
            if (AchivementQuest[i].State == QuestState.Done)
            {
                return true;
            }
        }

        return false;

    }

    public void SetImage(QuestData newQuest, Action<QuestData> onComplete = null)
    {
        int loadCount = 0;

        Managers.Resource.LoadAssetAsync<Sprite>("GoldImage", sprite =>
        {
            newQuest.GoldSprite = sprite;
            loadCount++;
            if (loadCount == 2) onComplete?.Invoke(newQuest);
        });

        Managers.Resource.LoadAssetAsync<Sprite>("GemImage", sprite =>
        {
            newQuest.GemSprte = sprite;
            loadCount++;
            if (loadCount == 2) onComplete?.Invoke(newQuest);
        });
    }


}
