using System.Collections.Generic;
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
        //돈 차감(연챠)
        if (callResults.Count > 1)
        {
            Managers.Player.AddGem(-(callResults.Count) * 9 * 30); // 0.9f * 300 = 9 * 30
        }
        //돈 차감(단챠)
        else
        {
            Managers.Player.AddGem(-(callResults.Count) * 300);
        }
            

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
        //돈 차감(연챠)
        if (callResults.Count > 1)
        {
            Managers.Player.AddGem(-(callResults.Count) * 9 * 30); // 0.9f * 300 = 9 * 30
        }
        //돈 차감(단챠)
        else
        {
            Managers.Player.AddGem(-(callResults.Count) * 300);
        }

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
    }
}
