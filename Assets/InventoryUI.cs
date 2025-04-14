using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : UI_Scene
{

    enum InventoryObj
    {
        Panel,
        Buttons,
        MenuTab,
    }

    // 필요한 정보들
    // 탭, 버튼, 슬롯
    private List<Slot> slots;
    private Button[] buttons;
    //private Tap tap;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(InventoryObj));

        if(slots == null)
        {
            slots = new List<Slot>();
            // slots.Capacity = (Enums.UnitType.Count > Enums.TowerType.Count) ? Enums.UnitType.Count : Enums.TowerType.Count;
        }

        // 슬롯 공간 확장했으면 Panel 하위 오브젝트정보들을 삽입
        Transform panel = GetObject((int)InventoryObj.Panel).transform;
        for (int i = 0; i < panel.childCount; i++)
        {
            slots.Add(panel.GetChild(i).GetComponent<Slot>());
            slots[i].Index = i;
            // 확인 디버깅
            Util.Log($"{slots[i].Index}");
        }
    }
}
