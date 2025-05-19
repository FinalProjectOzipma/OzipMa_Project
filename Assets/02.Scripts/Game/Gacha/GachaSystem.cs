using Firebase;
using Firebase.Extensions;
using Firebase.Functions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class GachaResultWrapper
{
    public List<GachaResult> results;
}

[System.Serializable]
public class GachaResult
{
    public int grade;
    public int id;
    public bool guaranteed; // 없어도 false로 기본 처리됨
}

public class GachaSystem
{
    public static GachaSystem Instance { get; private set; }
    public bool IsReady = false;

    private Dictionary<string, object> callData = new();

    FirebaseFunctions functions;

    public GachaSystem()
    {
        Instance = this;

        callData = new Dictionary<string, object>
        {
            { "gradeRanges", new int[4] { 0, 0, 0, 0 } }, 
            { "drawCount", 1 }                               
        };

        Init();
    }

    public void Init()
    {
        if (functions != null) return;

        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                functions = FirebaseFunctions.DefaultInstance;
                IsReady = true;
                Util.Log("Firebase 준비 완료!");
            }
            else
            {
                Util.LogError("Firebase 초기화 실패: " + task.Result.ToString());
            }
        });
    }

    public void SettingGradeRanges(int count, bool isUnit)
    {
        int offset = isUnit ? 0 : 4;
        List<DefaultTable.Gacha> result = Util.TableConverter<DefaultTable.Gacha>(Managers.Data.Datas[Enums.Sheet.Gacha]);

        callData["gradeRanges"] = new int[]
        {
            result[offset + 0].Key.Count,
            result[offset + 1].Key.Count,
            result[offset + 2].Key.Count,
            result[offset + 3].Key.Count
        };

        callData["drawCount"] = count;
    }

    public void CallGacha(int count, bool isUnit, Action<List<GachaResult>> onResult = null)
    {
        if (functions == null)
        {
            Init();
        }

        // Cloud Function에 넘길 데이터
        SettingGradeRanges(count, isUnit); 

        functions.GetHttpsCallable("gachaDrawWithGuarantees")
            .CallAsync(callData)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Util.LogError("가챠 실패: " + task.Exception);
                    return;
                }

                // Data를 JSON 문자열로 변환 (바로 변환이 어려워서 이런 방식으로 결정)
                string json = JsonConvert.SerializeObject(task.Result.Data);
                // JSON 역직렬화
                GachaResultWrapper parsed = JsonConvert.DeserializeObject<GachaResultWrapper>(json);
                foreach (GachaResult r in parsed.results)
                {
                    Util.Log($"등급: {r.grade}, ID: {r.id}" + (r.guaranteed ? "[확정]" : ""));
                }
                onResult?.Invoke(parsed.results);
            });

    }


    /// <summary>
    /// 단일 유닛뽑기
    /// </summary>
    /// <returns></returns>
    public MyUnit GetRandomUnit()
    {
        int rand = Random.Range(0, 100);
        RankType selectedRank;

        //랭크뽑기
        if (rand < 70) selectedRank = RankType.Normal; //70%

        else if (rand < 90) selectedRank = RankType.Rare; //20%
        else if (rand < 98) selectedRank = RankType.Epic; //8%
        else selectedRank = RankType.Legend; //2%

        return GetSelectUnit(selectedRank);
    }

    /// <summary>
    /// 단일 타워뽑기
    /// </summary>
    /// <returns></returns>
    public Tower GetRandomTower()
    {
        int rand = Random.Range(0, 100);
        RankType selectedRank;

        //랭크뽑기
        if (rand < 70) selectedRank = RankType.Normal; //70%
        else if (rand < 90) selectedRank = RankType.Rare; //20%
        else selectedRank = RankType.Epic; //TODO: 임시로 쓰는것. 랭크 추가시 지울것
        //else if (rand < 98) selectedRank = RankType.Epic; //8%
        //else selectedRank = RankType.Legend; //2%

        return GetSelectTower(selectedRank);
    }

    /// <summary>
    /// 특정 랭크 유닛 뽑기
    /// </summary>
    /// <param name="rank"></param>
    /// <returns></returns>
    public MyUnit GetSelectUnit(RankType rank)
    {
        //해당 랭크 중에서 유닛id뽑기
        var result = Util.TableConverter<DefaultTable.Gacha>(Managers.Data.Datas[Enums.Sheet.Gacha]);
        var row = result[(int)rank];
        int key = row.Key[Random.Range(0, row.Key.Count)]; //primarykey가져오기

        //유닛 데이터 로드해서 뽑기
        var unitData = Util.TableConverter<DefaultTable.MyUnit>(Managers.Data.Datas[Enums.Sheet.MyUnit]);
        string name = unitData[key].Name;

        MyUnit returnValue = new();

        Managers.Resource.LoadAssetAsync<GameObject>($"{name}_Brain", (prefab) =>
        {
            MyUnit unit = new();
            Sprite sprite = prefab.GetComponent<MyUnitController>().sprite;
            unit.Init(key, sprite);
            unit.Status.Attack.SetResearchMultiple(Managers.Player.AttackPercentResearch);
            unit.Status.Defence.SetResearchMultiple(Managers.Player.DefencePercentResartch);
            returnValue = unit;
        });

        return returnValue;
    }

    /// <summary>
    /// 특정 랭크 타워 뽑기
    /// </summary>
    /// <param name="rank"></param>
    /// <returns></returns>
    public Tower GetSelectTower(RankType rank)
    {
        //해당 랭크 중에서 유닛id뽑기
        var result = Util.TableConverter<DefaultTable.Gacha>(Managers.Data.Datas[Enums.Sheet.Gacha]);
        //타워는 4칸 아래에 데이터가 있으니까
        var row = result[(int)rank + 4];
        int key = row.Key[Random.Range(0, row.Key.Count)]; //primarykey가져오기

        //타워 데이터 로드해서 뽑기
        var towerData = Util.TableConverter<DefaultTable.Tower>(Managers.Data.Datas[Enums.Sheet.Tower]);
        string name = towerData[key].Name;

        Tower returnValue = new();

        Managers.Resource.LoadAssetAsync<GameObject>($"{name}Tower", (prefab) =>
        {
            Tower tower = new Tower();
            tower.Init(key, prefab.GetComponent<TowerControlBase>().Preview);
            tower.Status.Attack.SetResearchMultiple(Managers.Player.AttackPercentResearch);
            returnValue = tower;
        });

        return returnValue;
    }
}