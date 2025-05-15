using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DSlot : UI_Base
{


    [SerializeField] private GameObject Normal_Slot;
    [SerializeField] private GameObject Rare_Slot;
    [SerializeField] private GameObject Epic_Slot;
    [SerializeField] private GameObject Legendary_Slot;
    [SerializeField] private GameObject Myth_Slot;
    [SerializeField] public GameObject Screen;
    [SerializeField] public Button Button;
    [SerializeField] private Image Icon;

    public int Index { get; set; }

    public IGettable Gettable;

    private InventoryUI inventoryUI;

    private UI_InfoPopup objectInfo;

    private int itemKey { get; set; }


    private void Start()
    {
        Button.onClick.AddListener(SelectItem);
        inventoryUI = Managers.UI.GetScene<InventoryUI>();
    }


    public void SelectItem()
    {
        UserObject userObject = Gettable as UserObject;

        Managers.UI.ShowPopupUI<UI_InfoPopup>("InfoUI");
        objectInfo = Managers.UI.GetPopup<UI_InfoPopup>();

        if (userObject is MyUnit myUnit)
        {
            objectInfo.SelectedInfo(myUnit);
        }
        else if (userObject is Tower tower)
        {
            objectInfo.SelectedInfo(tower);
        }
    }


    public void SetData<T>(IGettable gettable) where T : UserObject
    {
        Gettable = gettable;
        T obj = gettable.GetClassAddress<T>();
        itemKey = obj.PrimaryKey;
        SelectedSlot(obj);
        Icon.sprite = obj.Sprite;      
    }


    private void SelectedSlot<T>(T go) where T : UserObject
    {
        RankType rank = go.RankType;

        // 모든 슬롯 비활성화
        Normal_Slot.SetActive(false);
        Rare_Slot.SetActive(false);
        Epic_Slot.SetActive(false);
        Legendary_Slot.SetActive(false);
        Myth_Slot.SetActive(false);

        // 선택된 슬롯만 활성화
        switch (rank)
        {
            case RankType.Normal:
                Normal_Slot.SetActive(true);
                break;
            case RankType.Rare:
                Rare_Slot.SetActive(true);
                break;
            case RankType.Epic:
                Epic_Slot.SetActive(true);
                break;
            case RankType.Legend:
                Legendary_Slot.SetActive(true);
                break;
            case RankType.Myth:
                Myth_Slot.SetActive(true);
                break;
        }
    }

}
