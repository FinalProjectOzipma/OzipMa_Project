using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : UI_Scene
{

    public enum InventoryObj
    {
        UserObjects,
        Buttons,
        MenuTab,
    }

    // 필요한 정보들
    // 탭, 버튼, 슬롯
    private Inventory data;
    private List<Slot> slots = new();

    private Transform myUnitPanel;
    private Button[] buttons;
    //private Tap tap;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        data = Managers.Game.inven;
        slots = new List<Slot>(); // UI측면
        Bind<GameObject>(typeof(InventoryObj));
        Refresh<Tower>();
    }

    private void Refresh<T>() where T : IGettable
    {
        slots.Clear();
        List<IGettable> selectedList = data.GetList<T>();

        Transform trans = GetObject((int)InventoryObj.UserObjects).transform; // 부모 객체 얻어오기

        int cnt = 0;
        for (int i = 0; i < selectedList.Count; i++)
        {
            trans.GetChild(i).gameObject.SetActive(true); // 비활성화 있을 수 있으니 활성화
            slots.Add(trans.GetOrAddComponent<Slot>());
            slots[i].SetData(selectedList[i]);
            cnt++;
        }

        while(cnt < trans.childCount) // 만약 이전에 슬롯이 필요없는 상황이면 비활성화
        {
            trans.GetChild(cnt++).gameObject.SetActive(false);
        }
    }

    private void SetSlot<T>(Transform trans, int index) where T : IGettable
    {
        trans.SetParent(trans);
        Slot slot = trans.GetOrAddComponent<Slot>();
        slots.Add(slot);
        slot.Index = index;   
    }
}
