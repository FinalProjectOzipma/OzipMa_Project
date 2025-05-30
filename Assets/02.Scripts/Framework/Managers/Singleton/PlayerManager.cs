using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  유저가 저장해야될 필수적인 요소들을 저장하는 곳
/// </summary>
public class PlayerManager
{
    // 튜토리얼 관련
    public Enums.TutorialStep LastTutorialStep = Enums.TutorialStep.None;
    public bool HasReceivedTutorialGold = false;
    public bool HasReceivedTutorialGem = false;

    public DateTime Last_LoginTime { get; set; }
    public int consecutive_days = 0;

    public Core MainCoreData { get; set; }
    public long Gold { get; set; }
    public long Gem { get; set; }

    public event Action<long> OnGoldChanged;
    public event Action<long> OnZamChanged;
    public event Action<int, int> OnStageChanged;

    public Inventory Inventory { get; set; } = new Inventory();

    // 스테이지 키
    public int CurrentKey
    {
        get => currentStageKey;
        set
        {
            currentStageKey = value;
            BuildingSystem.Instance.UpdateTowerCount();
        }
    }
    private int currentStageKey;
    public int CurrentStage { get; set; }
    public int CurrentWave { get; set; }

    public Dictionary<string, TowerStatus> TowerInfos; // 키 - $"Tower{PrimaryKey}" 형태가 될 것
    public Dictionary<string, MyUnitStatus> MyUnitInfos;
    public Dictionary<string, int> GridObjectMap; // 키 - Vector3Int를 ToString()한 것이 들어감 

    public ResearchData AttackResearchData { get; set; }
    public ResearchData DefenceResearchData { get; set; }
    public ResearchData CoreResearchData { get; set; }
    public ResearchData RandomResearchData { get; set; }

    public Dictionary<string, QuestData> QuestDataInfo { get; set; }

    public string RewordStartTime = "";
    public string LastRestQuestTime = "";

    public float AttackPercentResearch = 1.0f;
    public float DefencePercentResartch = 1.0f;


    public void Initialize()
    {
        // 처음 시작할때 선언
        Inventory = new Inventory();
        MainCoreData = new Core();

        AttackResearchData = new(ResearchUpgradeType.Attack);
        DefenceResearchData = new(ResearchUpgradeType.Defence);
        CoreResearchData = new(ResearchUpgradeType.Core);
        // 저장된게 있으면 선언

        // 저장된게 있으면 선언
        // Inventory = 가져오는거

        //SetGoldAndJame();
    }

    /// <summary>
    /// 골드 추가
    /// </summary>
    public void AddGold(long amount)
    {
        Gold += amount;
        OnGoldChanged?.Invoke(Gold);
    }

    /// <summary>
    /// 잼 추가
    /// </summary>
    public void AddGem(long amount)
    {
        Gem += amount;
        OnZamChanged?.Invoke(Gem);
    }

    /// <summary>
    /// 골드 소모
    /// </summary>
    public void SpenGold(long amount)
    {
        if (Gold < amount)
        {
            Managers.UI.Notify("골드가 부족합니다.", false);
            return;
        }

        Gold -= amount;

        if (Gold <= 0) Gold = 0;

        OnGoldChanged?.Invoke(Gold);
    }

    /// <summary>
    /// 잼 소모
    /// </summary>
    public void SpenGem(long amount)
    {
        if (Gem < amount)
        {
            Managers.UI.Notify("잼이 부족합니다.", false);
            return;
        }

        Gem -= amount;

        if (Gem <= 0) Gold = 0;

        OnZamChanged?.Invoke(Gem);
    }

    /// <summary>
    /// 다른 곳에서 골드 사용하기
    /// </summary>
    public long GetGold() => Gold;

    /// <summary>
    /// 다른 곳에서 잼 사용하기
    /// </summary>
    public long GetZam() => Gem;


    public void OnStageWave()
    {
        OnStageChanged?.Invoke(CurrentStage, CurrentWave);
    }

    public string GetStage() => CurrentStage.ToString();

    public void SaveInit()
    {
        // 1. Tower의 동적 데이터 저장 목적
        TowerInfos = new();
        List<IGettable> towers = Inventory.GetList<Tower>();
        if (towers != null)
        {
            foreach (var item in towers)
            {
                Tower tower = (Tower)item;
                TowerInfos.Add($"Tower{tower.PrimaryKey.ToString()}", tower.TowerStatus);
            }
        }

        // 2. MyUnit의 동적 데이터 저장 목적 
        MyUnitInfos = new();
        List<IGettable> myUnits = Inventory.GetList<MyUnit>();
        if (myUnits != null)
        {
            foreach (var item in myUnits)
            {
                MyUnit myUnit = (MyUnit)item;
                MyUnitInfos.Add($"MyUnit{myUnit.PrimaryKey.ToString()}", myUnit.Status as MyUnitStatus);
            }
        }

        // 3. 배치된 오브젝트 데이터 저장
        GridObjectMap = new();
        foreach (Vector3Int point in BuildingSystem.Instance.GridObjectMap?.Keys)
        {
            GridObjectMap.Add(point.ToString(), BuildingSystem.Instance.GridObjectMap[point]);
        }

        // 4. 퀘스트 데이터 저장
        QuestDataInfo = new();
        QuestDataInfo = Managers.Quest.ConvertToFlatDictionary();


    }

    public void LoadPlayerData(PlayerManager data)
    {
        // ===== MainCore는 오프라인 진행도 계산 후에 적용해야하나?????? =====
        //MainCoreData.Health = data.MainCoreData.Health;
        //MainCoreData.MaxHealth = data.MainCoreData.MaxHealth;
        //MainCoreData.CoreLevel = data.MainCoreData.CoreLevel;

        Last_LoginTime = data.Last_LoginTime;
        consecutive_days = data.consecutive_days;

        LastTutorialStep = data.LastTutorialStep;
        HasReceivedTutorialGold = data.HasReceivedTutorialGold;
        HasReceivedTutorialGem = data.HasReceivedTutorialGem;

        Gold = 0;
        Gem = 0;

        AddGem(data.Gem);
        AddGold(data.Gold);

        CurrentKey = data.CurrentKey;
        CurrentStage = data.CurrentStage;
        CurrentWave = data.CurrentWave;
        OnStageWave();

        TowerInfos = data.TowerInfos;
        MyUnitInfos = data.MyUnitInfos;

        RewordStartTime = data.RewordStartTime;
        LastRestQuestTime = data.LastRestQuestTime;

        AttackPercentResearch = data.AttackPercentResearch;
        DefencePercentResartch = data.DefencePercentResartch;

        QuestDataInfo = data.QuestDataInfo;

        if (data.MainCoreData != null)
        {
            MainCoreData = data.MainCoreData;
            Util.Log("코어레벨_Load" + MainCoreData.CoreLevel.GetValue().ToString());
        }




        // ===== 연구 정보=====
        if (data.AttackResearchData != null)
        {
            AttackResearchData = data.AttackResearchData;
        }

        if (data.DefenceResearchData != null)
        {
            DefenceResearchData = data.DefenceResearchData;
        }

        if (data.CoreResearchData != null)
        {
            CoreResearchData = data.CoreResearchData;
        }

        if (data.RandomResearchData != null)
        {
            RandomResearchData = data.RandomResearchData;
        }

        // ===== 인벤토리에 추가 - 타워 =====
        if (TowerInfos != null)
        {
            Inventory.ClearList<Tower>();
            List<DefaultTable.Tower> Towers = Util.TableConverter<DefaultTable.Tower>(Managers.Data.Datas[Enums.Sheet.Tower]);
            foreach (string Key in TowerInfos.Keys)
            {
                int primaryKey = int.Parse(Key.Replace("Tower", ""));
                Managers.Resource.LoadAssetAsync<GameObject>($"{Towers[primaryKey].Name}Tower", original =>
                {
                    Tower tower = new Tower();
                    tower.Init(primaryKey, original.GetComponent<TowerControlBase>().Preview);
                    tower.TowerStatus.SetDatas(TowerInfos[Key]); // 동적 정보 넘김
                    Inventory.Add<Tower>(tower);
                });
            }

            // ===== 원래 배치대로 타워 배치 =====
            if (data.GridObjectMap != null)
            {
                Dictionary<Vector3Int, int> convertedMap = new();
                foreach (string pointStr in data.GridObjectMap.Keys)
                {
                    if (Util.TryStringToVector3Int(pointStr, out Vector3Int res) == true)
                    {
                        Util.Log($"LoadPlayerData의 여기가 가끔 안돼요 : {res.ToString()}");
                        convertedMap.Add(res, data.GridObjectMap[pointStr]);
                    }
                }

                BuildingSystem.Instance.BuildingInit(convertedMap);
            }
            else
            {
                Util.Log("GameData의 GridObjectMap이 Null입니다.");
            }
        }

        // ===== 인벤토리에 추가 - 아군 유닛 =====
        if (MyUnitInfos != null)
        {
            Inventory.ClearList<MyUnit>();
            List<DefaultTable.MyUnit> MyUnits = Util.TableConverter<DefaultTable.MyUnit>(Managers.Data.Datas[Enums.Sheet.MyUnit]);
            foreach (string Key in MyUnitInfos.Keys)
            {
                int primaryKey = int.Parse(Key.Replace("MyUnit", ""));
                Managers.Resource.LoadAssetAsync<GameObject>($"{MyUnits[primaryKey].Name}_Brain", original =>
                {
                    MyUnit unit = new MyUnit();
                    unit.Init(primaryKey, original.GetComponent<MyUnitController>().sprite);
                    (unit.Status as MyUnitStatus).SetDatas(MyUnitInfos[Key]); // 동적 정보 넘김
                    Inventory.Add<MyUnit>(unit);
                });
            }
        }

        //===== 퀘스트 데이터 로드 =====
        if (QuestDataInfo != null)
        {
            Managers.Quest.QuestDatas.Clear();

            foreach (var kvp in QuestDataInfo)
            {
                QuestType type = kvp.Value.Type;

                if (!Managers.Quest.QuestDatas.ContainsKey(type))
                    Managers.Quest.QuestDatas[type] = new List<QuestData>();

                Managers.Quest.QuestDatas[type].Add(kvp.Value);
                Managers.Quest.SetImage(kvp.Value);

            }

            Managers.Quest.ResetEtcQuest();

        }
    }
}

public enum ResearchUpgradeType
{
    Attack,
    Defence,
    Core
}


public class ResearchData
{
    public ResearchUpgradeType type;
    public string StartTime;
    public float ResearchDuration;
    public int UpdateLevel;
    public float UpdateStat;
    public long SpendGold;
    public long SpendGem;
    public float SecondsToReduce;


    public ResearchData(ResearchUpgradeType _type)
    {
        type = _type;
        StartTime = "";
        ResearchDuration = 0;
        UpdateLevel = 0;
        UpdateStat = 0.0f;
        SpendGold = 0;
        SpendGem = 0;
        SecondsToReduce = 0;
    }
}

