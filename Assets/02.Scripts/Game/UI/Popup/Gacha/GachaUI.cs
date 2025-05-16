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

    [SerializeField] private RectTransform RectTransform;

    private GachaSystem gacha = new();
    private List<IGettable> result;


    private void Start()
    {
        Init();
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
        if (Managers.Player.Gem < num * 300)
        {
            Managers.UI.Notify("잼이 부족합니다.", false);
            return;
        }
        //돈 차감
        Managers.Player.AddGem(-num * 300);

        result = new();

        //10연뽑시 에픽 1개 확정
        if (num == 10)
        {
            num -= 1;
            var res = gacha.GetSelectUnit(RankType.Epic);
            result.Add(res);
            Managers.Player.Inventory.Add<MyUnit>(res);
        }
        //100연뽑시 전설 1개 확정
        else if (num == 100)
        {
            num -= 1;
            var res = gacha.GetSelectUnit(RankType.Legend);
            result.Add(res);
            Managers.Player.Inventory.Add<MyUnit>(res);
        }

        //나머지는 가챠돌리기
        for (int i = 0; i < num; i++)
        {
            var res = gacha.GetRandomUnit();
            result.Add(res);
            Managers.Player.Inventory.Add<MyUnit>(res);
        }

        //결과 추출
        Managers.Resource.Instantiate("GachaResultUI", (go) =>
        {
            UI_GachaResult res = go.GetComponent<UI_GachaResult>();
            res.ShowResult(result);
        });
    }

    private void TowerOnClick(int num)
    {
        if (Managers.Player.Gem < num * 300)
        {
            Managers.UI.Notify("잼이 부족합니다.", false);
            return;
        }
        Managers.Player.AddGem(-num * 300);

        result = new();

        if (num == 10)
        {
            num -= 1;
            var res = gacha.GetSelectTower(RankType.Epic);
            result.Add(res);
            Managers.Player.Inventory.Add<Tower>(res);
        }
        else if (num == 100)
        {
            Util.Log("우왕 레전더리 하지만 없는걸...");
            //num -= 1;
            //gacha.GetSelectTower(RankType.Legend);
            //Managers.Player.Inventory.Add<Tower>(res);
        }

        for (int i = 0; i < num; i++)
        {
            var res = gacha.GetRandomTower();
            result.Add(res);
            Managers.Player.Inventory.Add<Tower>(res);
        }

        Managers.Resource.Instantiate("GachaResultUI", (go) =>
        {
            go.GetComponent<UI_GachaResult>().ShowResult(result);
        });
    }
}
