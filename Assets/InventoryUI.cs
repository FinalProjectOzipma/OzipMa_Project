using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : UI_Scene
{

    enum InventoryObj
    {
        MyUnit,// IGettable 상속받은 클래스 이름과 하이러키창에 이름 같게 설정해줘야됨
        Tower, // IGettable 상속받은 클래스 이름과 하이러키창에 이름 같게 설정해줘야됨
        Buttons,
        MenuTab,
    }

    // 필요한 정보들
    // 탭, 버튼, 슬롯
    private Inventory data;
    private Dictionary<string, List<Slot>> slotDict = new();

    private Transform myUnitPanel;
    private Button[] buttons;
    //private Tap tap;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        data = new Inventory();
        slotDict[nameof(Tower)] = new List<Slot>(); // UI측면
        slotDict[nameof(MyUnit)] = new List<Slot>(); // UI측면
        Bind<GameObject>(typeof(InventoryObj));
    }

    private void Refresh<T>() where T : IGettable
    {
        if(Enum.TryParse(typeof(InventoryObj), nameof(T), out var enumResult) == false)
        {
            Util.LogWarning($"Dont receive nameof({nameof(T)}) Enum");
            return;
        }

        int prevListCount = slotDict[nameof(T)].Count; 
        List<T> selectedList = data.GetList<T>();
        
        // List가 부족할때 생성
        for(int i = prevListCount; i< selectedList.Count; i++)
        {
            // Slot오브젝트가 없으면 만들어줘야됨
            Managers.Resource.Instantiate(nameof(Slot), (go) => { SetSlot<T>(GetObject((int)enumResult).transform, i); });
        }

        // 가지고있는 것보다 슬롯의 오브젝트가 많으면 비활성화
        for (int i = selectedList.Count; i < prevListCount; i++)
        {
            // Slot오브젝트가 없으면 만들어줘야됨
            Managers.Resource.Destroy(slotDict[nameof(T)][i].gameObject);
        }

        // List정보 변경, 위에서 비동기적으로 돌리고있으니 꼬일 수도 있겠다
        /*for (int i = 0; i < selectedList.Count; i++)
        {
            slotDict[nameof(T)][i].SetData<T>(selectedList[i]);
        }*/
    }

    private void SetSlot<T>(Transform trans, int index) where T : IGettable
    {
        trans.SetParent(trans);
        Slot slot = trans.GetOrAddComponent<Slot>();
        slot.Index = index;   
    }
}
