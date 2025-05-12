using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using VInspector.Libs;

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
        Util.Log("초기화 히얍!");
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
        result = new();

        for (int i = 0; i < num; i++)
        {
            result.Add(gacha.GetRandomTower());
        }
        //TODO: 인벤토리에 넣어주는 작업 필요

        Managers.Resource.Instantiate("GachaResultUI", (go)=>
        {
            go.GetComponent<UI_GachaResult>().ShowResult(result);
        });
    }

    private void UnitOnClick(int num)
    {
        result = new();

        for (int i = 0; i < num; i++)
        {
            var res = gacha.GetRandomUnit();
            result.Add(res);
            Managers.Player.Inventory.Add<MyUnit>(res);
        }

        Util.Log("인벤에 넣어드렸습니다");
        Managers.Resource.Instantiate("GachaResultUI", (go) =>
        {
            Util.Log("결과창 생성해드렸습니다~");
            go.transform.SetParent(RectTransform, false);
            go.GetComponent<UI_GachaResult>().ShowResult(result);
        });
    }
}
