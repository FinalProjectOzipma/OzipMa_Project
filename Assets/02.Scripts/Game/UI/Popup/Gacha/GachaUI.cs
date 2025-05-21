using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class GachaUI : UI_Popup
{
    [SerializeField] private Button MyUnitSingleButton;
    [SerializeField] private Button MyUnitDualButton;
    [SerializeField] private Button MyUnitHundredButton;
    [SerializeField] private Button TowerSingleButton;
    [SerializeField] private Button TowerDualButton;
    [SerializeField] private Button TowerHundredButton;
    [SerializeField] private Button BGClose;

    [SerializeField] public RectTransform RectTransform;

    private GachaSystem gacha;
    private List<IGettable> result;

    private bool isGachaInProgress = false; // 가챠 중복 방지

    private void Start()
    {
        Init();
        AnimePopup(RectTransform.gameObject);
        gacha = GachaSystem.Instance;
    }

    private void OnEnable()
    {
        AnimePopup(RectTransform.gameObject);
    }

    public override void Init()
    {
        MyUnitSingleButton.onClick.AddListener(() => UnitOnClick(1));
        MyUnitDualButton.onClick.AddListener(() => UnitOnClick(10));
        MyUnitHundredButton.onClick.AddListener(() => UnitOnClick(100));
        TowerSingleButton.onClick.AddListener(() => TowerOnClick(1));
        TowerDualButton.onClick.AddListener(() => TowerOnClick(10));
        TowerHundredButton.onClick.AddListener(() => TowerOnClick(100));
        BGClose.gameObject.BindEvent((Managers.UI.GetScene<UI_Main>().OnClickGacha));
    }
    private void UnitOnClick(int num)
    {
        if (isGachaInProgress) return;
        if (Managers.Player.Gem < num * 300)
        {
            Managers.UI.Notify("잼이 부족합니다.", false);
            return;
        }

        isGachaInProgress = true;
        // 서버에서 데이터 받아서 실행
        gacha.CallGacha(num, true, GetUnitGachaResult);
    }

    /// <summary>
    /// 서버에서 값 받아온 이후에 실행됨
    /// </summary>
    /// <param name="callResults">뽑힌 데이터</param>
    private void GetUnitGachaResult(List<GachaResult> callResults/*등급, id, 확정인지 여부*/)
    {
        //돈 차감
        Managers.Player.AddGem(-(callResults.Count) * 300);

        //뽑힌 유닛 넣어주기 
        result = new();
        foreach (GachaResult item in callResults)
        {
            MyUnit res = gacha.GetSelectUnit((RankType)item.grade, item.id);
            result.Add(res);
            Managers.Player.Inventory.Add<MyUnit>(res);
        }

        //결과 보여주기
        Managers.Resource.Instantiate("GachaResultUI", (go) =>
        {
            UI_GachaResult res = go.GetComponent<UI_GachaResult>();
            res.ShowResult(result);
            isGachaInProgress = false;
        });

        // 애널리틱스
        #region gacha_used
        StringBuilder getUnitBuilder = new StringBuilder();
        StringBuilder gradeTotalBuilder = new StringBuilder();
        Dictionary<string, int> unitTotal = new();
        int[] gradeTotal = new int[(int)RankType.Count];

        // 유닛의 정보 string 변화 작업
        foreach (var unit in result)
        {
            MyUnit myUnit = unit.GetClassAddress<MyUnit>();
            ++gradeTotal[(int)myUnit.RankType];
            
            if(!unitTotal.TryAdd(myUnit.Name, 1))
                ++unitTotal[myUnit.Name];
        }

        foreach (var pair in unitTotal)
            getUnitBuilder.Append($"{pair.Key} : {pair.Value}/ ");
        

        // 등급 토탈 정보 string 변화 작업
        for(int i = 0; i < (int)RankType.Count; i++)
            gradeTotalBuilder.Append($"{Enum.GetName(typeof(RankType), i)} : {gradeTotal[i]}");
        

        Managers.Analytics.AnalyticsGachaUsed("MyUnit", "Gem", callResults.Count * 300, callResults.Count, getUnitBuilder.ToString(),
            gradeTotalBuilder.ToString());
        #endregion
    }

    private void TowerOnClick(int num)
    {
        if (isGachaInProgress) return;
        if (Managers.Player.Gem < num * 300)
        {
            Managers.UI.Notify("잼이 부족합니다.", false);
            return;
        }

        isGachaInProgress = true;
        // 서버에서 데이터 받아서 실행
        gacha.CallGacha(num, true, GetTowerGachaResult);
    }

    /// <summary>
    /// 서버에서 값 받아온 이후에 실행됨
    /// </summary>
    /// <param name="callResults">뽑힌 데이터</param>
    private void GetTowerGachaResult(List<GachaResult> callResults/*등급, id, 확정인지 여부*/)
    {
        //돈 차감
        Managers.Player.AddGem(-(callResults.Count) * 300);

        //뽑힌 유닛 넣어주기 
        result = new();
        foreach (GachaResult item in callResults)
        {
            Tower res = gacha.GetSelectTower((RankType)item.grade, item.id);
            result.Add(res);
            Managers.Player.Inventory.Add<Tower>(res);
        }

        //결과 보여주기
        Managers.Resource.Instantiate("GachaResultUI", (go) =>
        {
            UI_GachaResult res = go.GetComponent<UI_GachaResult>();
            res.ShowResult(result);
            isGachaInProgress = false;
        });


        // 애널리틱스
        #region gacha_used
        StringBuilder getTowerBuilder = new StringBuilder();
        StringBuilder gradeTotalBuilder = new StringBuilder();
        Dictionary<string, int> towerTotal = new();
        int[] gradeTotal = new int[(int)RankType.Count];

        // 유닛의 정보 string 변화 작업
        foreach (var towers in result)
        {
            Tower tower = towers.GetClassAddress<Tower>();
            ++gradeTotal[(int)tower.RankType];

            if (!towerTotal.TryAdd(tower.Name, 1))
                ++towerTotal[tower.Name];
        }

        foreach (var pair in towerTotal)
            getTowerBuilder.Append($"{pair.Key} : {pair.Value}/ ");

        // 등급 토탈 정보 string 변화 작업
        for (int i = 0; i < (int)RankType.Count; i++)
            gradeTotalBuilder.Append($"{Enum.GetName(typeof(RankType), i)} : {gradeTotal[i]}");


        Managers.Analytics.AnalyticsGachaUsed("Tower", "Gem", callResults.Count * 300, callResults.Count, getTowerBuilder.ToString(),
            gradeTotalBuilder.ToString());
        #endregion
    }
}
