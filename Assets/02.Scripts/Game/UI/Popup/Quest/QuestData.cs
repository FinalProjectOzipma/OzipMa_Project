using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class QuestData
{
    public int ID;
    public QuestType Type;
    public string Name;
    public string Description;

    public ConditionType ConditionType;
    public int TargetID;
    public int Goal;

    public int Progress;


    public QuestState State;

    public int RewardGem;

    public int IsActive;

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

    public QuestData(int id, QuestType type, string name, string description, ConditionType conditionType, int targetID, int goal, int rewardGem, int isActive)
    {
        ID = id;
        Type = type;
        Name = name;
        Description = description;
        ConditionType = conditionType;
        TargetID = targetID;
        Goal = goal;
        RewardGem = rewardGem;
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