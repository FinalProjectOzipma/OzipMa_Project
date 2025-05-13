using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
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
    [SerializeField] private Button CloseButton;

    [SerializeField] private RectTransform RectTransform;

    private GachaSystem gacha = new();
    private List<IGettable> result;

    private List<UI_GachaSlot> slots;


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
        CloseButton.onClick.AddListener(ClosePopupUI);
    }

    private void TowerOnClick(int num)
    {
        if (Managers.Player.Gem < num * 300)
        {
            Managers.Player.AddGem(- num * 300);
            Managers.UI.ShowPopupUI<UI_Alarm>("ZamAlarmPopup");
            return;
        }

        if (num == 10)
        {
            num -= 1;
            gacha.GetSelectTower(RankType.Epic);
        }
        else if (num == 100)
        {
            Util.Log("우왕 레전더리 하지만 없는걸...");
            //num -= 1;
            //gacha.GetSelectTower(RankType.Legend);
        }
        result = new();

        for (int i = 0; i < num; i++)
        {
            var res = gacha.GetRandomTower();
            result.Add(res);
            Managers.Player.Inventory.Add<Tower>(res);
        }

        Managers.Resource.Instantiate("GachaResultUI", (go)=>
        {
            go.GetComponent<UI_GachaResult>().ShowResult(result);
        });
    }

    private void UnitOnClick(int num)
    {
        if (Managers.Player.Gem < num * 300)
        {
            Managers.Player.AddGem(-num * 300);

            Managers.UI.ShowPopupUI<UI_Alarm>("ZamAlarmPopup");
            return;
        }

        if (num == 10)
        {
            Util.Log("우왕 에픽 하지만 없는걸...");
            num -= 1;
            gacha.GetSelectUnit(RankType.Epic);
        }
        else if (num == 100)
        {
            Util.Log("우왕 레전더리 하지만 없는걸...");
            num -= 1;
            gacha.GetSelectUnit(RankType.Legend);
        }
        result = new();

        for (int i = 0; i < num; i++)
        {
            var res = gacha.GetRandomUnit();
            result.Add(res);
            Managers.Player.Inventory.Add<MyUnit>(res);
        }
        Managers.Resource.Instantiate("GachaResultUI", (go) =>
        {
            UI_GachaResult res = go.GetComponent<UI_GachaResult>();
            res.ShowResult(result);
        });
    }
}
