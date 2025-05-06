using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
///  유저가 저장해야될 필수적인 요소들을 저장하는 곳
/// </summary>
public class PlayerManager 
{
    public Core MainCoreData { get; set; }
    public int Money { get; set; }
    public long gold { get; private set; }
    public long zam { get; private set; }

    public event Action<long> OnGoldChanged;
    public event Action<long> OnZamChanged;
    public event Action<int, int> OnStageChanged;

    private string myGoldKey = "myGold";
    private string myZamKey = "myZam";
    public Inventory Inventory { get; set; } = new Inventory();

    public int CurrentStage { get; set; }
    public int CurrentWave { get; set; }

    public Dictionary<int, TowerStatus> TowerInfos;
    public Dictionary<int, MyUnitStatus> MyUnitInfos;
    public Dictionary<Vector3Int, int> GridObjectMap;

    public void Initialize()
    {
        // 처음 시작할때 선언
        Inventory = new Inventory();
        MainCoreData = new Core();

        // 저장된게 있으면 선언
        // Inventory = 가져오는거

        SetGoldAndJame();
    }

    /// <summary>
    /// 골드 추가
    /// </summary>
    public void AddGold(long amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }

    /// <summary>
    /// 잼 추가
    /// </summary>
    public void AddZam(long amount)
    {
        zam += amount;
        OnZamChanged?.Invoke(zam);
    }

    /// <summary>
    /// 골드 소모
    /// </summary>
    public void SpenGold(long amount)
    {
        if (gold < amount)
        {
            Debug.Log("돈이 부족합니다");
            return;
        }

        gold -= amount;

        if (gold <= 0) gold = 0;

        OnGoldChanged?.Invoke(gold);
    }

    /// <summary>
    /// 잼 소모
    /// </summary>
    public void SpenZam(long amount)
    {
        if (zam < amount)
        {
            Debug.Log("돈이 부족합니다");
            return;
        }

        zam -= amount;

        if (zam <= 0) gold = 0;

        OnZamChanged?.Invoke(zam);
    }

    /// <summary>
    /// 다른 곳에서 골드 사용하기
    /// </summary>
    public long GetGold() => gold;

    /// <summary>
    /// 다른 곳에서 잼 사용하기
    /// </summary>
    public long GetZam() => zam;

    public void SaveEcomomy()
    {
        PlayerPrefs.SetString(myGoldKey, gold.ToString());
        PlayerPrefs.SetString(myZamKey, zam.ToString());
    }

    public void SetGoldAndJame()
    {
        gold = PlayerPrefs.HasKey(myGoldKey) ? long.Parse(PlayerPrefs.GetString(myGoldKey)) : 3000L;
        zam = PlayerPrefs.HasKey(myZamKey) ? long.Parse(PlayerPrefs.GetString(myZamKey)) : 1000L;
    }


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
        foreach (var item in towers)
        {
            Tower tower = (Tower)item;
            TowerInfos.Add(tower.PrimaryKey, tower.TowerStatus);
        }

        // 2. MyUnit의 동적 데이터 저장 목적 
        MyUnitInfos = new();
        List<IGettable> myUnits = Inventory.GetList<MyUnit>();
        foreach (var item in myUnits)
        {
            MyUnit myUnit = (MyUnit)item;
            MyUnitInfos.Add(myUnit.PrimaryKey, myUnit.Status as MyUnitStatus);
        }

        // 3. 배치된 오브젝트 데이터 저장
        GridObjectMap = BuildingSystem.Instance.GridObjectMap;
    }

    public void LoadPlayerData(PlayerManager data)
    {
        // ===== MainCore는 오프라인 진행도 계산 후에 적용해야하나?????? =====
        //MainCoreData.Health = data.MainCoreData.Health;
        //MainCoreData.MaxHealth = data.MainCoreData.MaxHealth;
        //MainCoreData.CoreLevel = data.MainCoreData.CoreLevel;
        
        Money = data.Money;
        gold = data.gold;
        zam = data.zam;

        CurrentStage = data.CurrentStage;
        CurrentWave = data.CurrentWave;

        TowerInfos = data.TowerInfos;
        MyUnitInfos = data.MyUnitInfos;
        BuildingSystem.Instance.BuildingInit(data.GridObjectMap);

        // ===== 인벤토리에 추가 =====
        List<DefaultTable.Tower> Towers = Util.TableConverter<DefaultTable.Tower>(Managers.Data.Datas[Enums.Sheet.Tower]);
        foreach (int primaryKey in TowerInfos.Keys)
        {
            Managers.Resource.LoadAssetAsync<GameObject>($"{Towers[primaryKey]}Tower", original => 
            {
                Tower tower = new Tower();
                tower.Init(primaryKey, original.GetComponent<TowerControlBase>().Preview);
                Inventory.Add<Tower>(tower);
            });
        }
        List<DefaultTable.MyUnit> MyUnits = Util.TableConverter<DefaultTable.MyUnit>(Managers.Data.Datas[Enums.Sheet.MyUnit]);
        foreach(int primaryKey in MyUnitInfos.Keys)
        {
            Managers.Resource.LoadAssetAsync<GameObject>($"{MyUnits[primaryKey]}_Brain", original => 
            {
                MyUnit unit = new MyUnit();
                unit.Init(primaryKey, original.GetComponent<MyUnitController>().sprite);
                Inventory.Add<MyUnit>(unit);
            });
        }
    }
}
