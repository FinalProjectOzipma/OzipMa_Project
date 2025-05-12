using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using VInspector.Libs;

public class UI_Gacha : UI_Popup
{
    [SerializeField] private List<Button> TowerGachaBtn;
    [SerializeField] private List<Button> UnitGachaBtn;

    private GachaSystem gacha;
    private List<IGettable> result;

    private List<UI_GachaSlot> slots;

    

    private void Start()
    {
        Init();
    }

    public void TowerOnClick(int num)
    {
        result = new();

        for (int i = 0; i < num; i++)
        {
            result.Add(gacha.GetRandomTower());
        }
        //TODO: 인벤토리에 넣어주는 작업 필요

        Managers.Resource.Instantiate("GachaResult");
    }
    
    public void UnitOnClick(int num)
    {
        result = new();

        for (int i = 0; i < num; i++)
        {
            result.Add(gacha.GetRandomUnit());
        }
        //TODO: 인벤토리에 넣어주는 작업 필요

        Managers.Resource.Instantiate("GachaResult");
    }
}
