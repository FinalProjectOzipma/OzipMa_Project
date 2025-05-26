using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class QuestData
{
    public int ID; // 퀘스트 식별번호
    public QuestType Type; // 퀘스트 타입, Daily,Achivement, Repeat
    public string Name; // 퀘스트 이름
    public string Description; // 퀘스트 설명

    public ConditionType ConditionType; // 퀘스트 종류, Bosskill, EnemyKill....
    public int TargetID; // 퀘스트 대상 -1 대상 상관 없음
    public int Goal; // 퀘스트 달성 목표치

    public int Progress; // 퀘스트 진행률


    public QuestState State; // 퀘스트의 상태, Doing, Done, Compelete

    public int RewardGem; // 퀘스트 보상 잼

    public int RewardGold; // 퀘스트 보상 골드

    public byte IsActive; // 퀘스트 활성화 여부

    [JsonIgnore]
    public Sprite GemSprte;

    [JsonIgnore]
    public Sprite GoldSprite;

    [JsonIgnore]
    public Action OnProgressChanged;

    [JsonIgnore]
    public Action<QuestState> OnStateChanged;

    public void AddProgress(int value)
    {
        Progress += value;
        OnProgressChanged?.Invoke();
    }

    public void SetProgress(int value)
    {
        Progress = value;
        OnProgressChanged?.Invoke();
    }


    public void CheckDone()
    {
        if (Progress >= Goal)
        {
            Progress = Goal;
            State = QuestState.Done;
            OnStateChanged?.Invoke(State);
        }
    }

    public QuestData(int id, QuestType type, string name, string description, ConditionType conditionType, int targetID, int goal, int rewardGem, int rewardGOld, byte isActive)
    {
        ID = id;
        Type = type;
        Name = name;
        Description = description;
        ConditionType = conditionType;
        TargetID = targetID;
        Goal = goal;
        RewardGem = rewardGem;
        RewardGold = rewardGOld;
        IsActive = isActive;

        State = QuestState.Doing;
        Progress = 0;

    }
}

public enum QuestState
{
    Doing,
    Done,
    Complete
}