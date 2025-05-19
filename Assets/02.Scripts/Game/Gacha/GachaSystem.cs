using Firebase;
using Firebase.Extensions;
using Firebase.Functions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// 서버에서 가챠 데이터 뽑기 
    /// </summary>
    /// <param name="count">count개만큼 뽑음</param>
    /// <param name="isUnit">true: 마이유닛, false: 타워</param>
    /// <param name="onResult">뽑기 데이터 로드 완료 후에 실행, 뽑기 데이터 이곳으로 반환됨</param>
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
                    //Util.Log($"등급: {r.grade}, ID: {r.id}" + (r.guaranteed ? "[확정]" : "")); // 테스트용
                }
                onResult?.Invoke(parsed.results);
            });

    }

    /// <summary>
    /// 특정 랭크 유닛 뽑기
    /// </summary>
    /// <param name="rank"></param>
    /// <returns></returns>
    public MyUnit GetSelectUnit(RankType rank, int id)
    {
        //rank 랭크 중 id에 해당하는 유닛 뽑아주기
        List<DefaultTable.Gacha> result = Util.TableConverter<DefaultTable.Gacha>(Managers.Data.Datas[Enums.Sheet.Gacha]);
        DefaultTable.Gacha row = result[(int)rank];
        int key = row.Key[id];

        //key 유닛 데이터 로드
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
    public Tower GetSelectTower(RankType rank, int id)
    {
        //Legend등급 이상의 타워가 아직 없어서 Epic으로 대체
        if ((int)rank >= (int)RankType.Legend)
        {
            rank = RankType.Epic;
        }

        //rank 랭크 중 id에 해당하는 유닛 뽑아주기
        List<DefaultTable.Gacha> result = Util.TableConverter<DefaultTable.Gacha>(Managers.Data.Datas[Enums.Sheet.Gacha]);
        DefaultTable.Gacha row = result[(int)rank + 4];
        int key = row.Key[id];

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