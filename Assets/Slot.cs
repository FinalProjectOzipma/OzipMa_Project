using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : UI_Scene, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private enum Images
    {
        Icon,
        StackGageFill,
        Selected
    }

    private enum TextMeshs
    {
        ObjInfo,
        StackText
    }

    private Button button; // 이녀석은 현재 들고 있는 컴포넌트객체니깐 그냥 Get으로 불러드림

    public int Index { get; set; }
    public bool IsActive { get; private set; } = false;

    public IGettable Gettable;

    private GameObject PreviewObj;
    private SpriteRenderer previewRenderer;
    private Sprite _sprite;
    private BuildingSystem buildingSystem;
    private InventoryUI inventoryUI;
    private Image _stackGage;
    private int itemKey { get; set; }

    private bool isSelect = false;

    private void Awake()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(TextMeshs));
        button = GetComponent<Button>();
        button.onClick.AddListener(SeletToggle);

        inventoryUI = Managers.UI.GetSceneList<InventoryUI>();
        GetImage((int)Images.Selected).gameObject.SetActive(false);
    }

    private void SeletToggle()
    {
        if (inventoryUI.CurrentState != InventoryUI.STATE.SELECTABLE) return;

        UserObject userObject = Gettable as UserObject;
        GameObject imgObj = GetImage((int)Images.Selected).gameObject;
        imgObj.SetActive(!imgObj.activeSelf);
        IsActive = imgObj.activeInHierarchy;
        inventoryUI.isSelect = true;

        if (userObject == null)
            return;

        bool isMaxLevel = userObject.Status.Level.GetValue() >= userObject.Status.MaxLevel.GetValue();

        if (IsActive)
        {
            // 선택 상태로 바뀜
            if (!isMaxLevel)
            {
                Managers.Upgrade.OnUpgradeGold(Managers.Upgrade.LevelUPGold);
                isSelect = true;
            }
            else
            {
                // 만렙이면 선택은 되지만 비용 증가 없음
                Util.Log("만렙 유닛 선택됨 (비용 없음)");
            }
        }
        else
        {
            // 선택 해제
            if (!isMaxLevel && isSelect)
            {
                Managers.Upgrade.OnUpgradeGold(-Managers.Upgrade.LevelUPGold);
                isSelect = false;
            }
        }

    }

    // 전체 선택될때 호출해야되는 메서드
    public void OnSelect()
    {
        if (inventoryUI.CurrentState != InventoryUI.STATE.SELECTABLE) return;

        UserObject userObject = Gettable as UserObject;

        if (IsActive)
        {
            IsActive = false;
            Managers.Upgrade.OnUpgradeGold(-Managers.Upgrade.LevelUPGold);
        }

        bool isMaxLevel = userObject.Status.Level.GetValue() >= userObject.Status.MaxLevel.GetValue();  

        if(!isMaxLevel)
        {
            IsActive = true;
            Managers.Upgrade.OnUpgradeGold(Managers.Upgrade.LevelUPGold);
            GetImage((int)Images.Selected).gameObject.SetActive(true);
        }   
    }

    public void DisSelect()
    {
        IsActive = false;
        GetImage((int)Images.Selected).gameObject.SetActive(false);
    }


    public void SetData<T>(IGettable gettable) where T : UserObject
    {
        Gettable = gettable;
        T obj = gettable.GetClassAddress<T>();
        itemKey = obj.PrimaryKey;
        var status = obj.Status;
        _sprite = obj.Sprite;
        GetImage((int)Images.Icon).sprite = _sprite;
        GetText((int)TextMeshs.ObjInfo).text = $"LV.{status.Level.GetValue()}\r\nEV.{status.Grade.GetValue()}";
        GetImage((int)Images.StackGageFill).fillAmount = status.Stack.GetValue() % status.MaxStack.GetValue();
        GetText((int)TextMeshs.StackText).text = $"{status.Stack.GetValue()}/{status.MaxStack.GetValue()}";
    }

    #region 드래그 배치
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(inventoryUI.CurrentState != InventoryUI.STATE.PUTABLE)
        {
            Managers.UI.ShowPopupUI<UI_Alarm>("BatchPopup");
            Util.Log("배치모드가 아니잖아!!");
            return;
        }
        buildingSystem = BuildingSystem.Instance;
        Vector2 inputPos = eventData.position;
        Vector3 cellWorldPos = buildingSystem.UpdatePosition(inputPos);
        cellWorldPos.y -= 0.2f;
        if (PreviewObj == null)
        {
            Managers.Resource.Instantiate("BuildingPreview", go => 
            {
                PreviewObj = go;
                PreviewObj.transform.position = cellWorldPos;
                previewRenderer = PreviewObj.GetComponent<SpriteRenderer>();
            });
        }
        else
        {
            PreviewObj.transform.position = cellWorldPos;
        }
        previewRenderer.sprite = _sprite;
        Managers.UI.GetSceneList<InventoryUI>().OnSwipe();
        buildingSystem.DragController.IsSlotDragging = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (inventoryUI.CurrentState != InventoryUI.STATE.PUTABLE) return;
        Vector2 inputPos = eventData.position;
        Vector3 cellWorldPos = buildingSystem.UpdatePosition(inputPos);
        cellWorldPos.y -= 0.3f;
        PreviewObj.transform.position = cellWorldPos;
        if(buildingSystem.CanTowerBuild(inputPos) == false)
        {
            previewRenderer.color = Color.red;
            return;
        }
        previewRenderer.color = Color.green;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (inventoryUI.CurrentState != InventoryUI.STATE.PUTABLE) return;
        Vector2 inputPos = eventData.position;
        Managers.Resource.Destroy(PreviewObj);
        PreviewObj = null;

        if (buildingSystem.CanTowerBuild(inputPos) == false)
        {
            // 배치 불가능하면 드래그 취소됨
            return;
        }

        // 배치 가능
        DefaultTable.Tower data = Managers.Data.GetTable<DefaultTable.Tower>(Enums.Sheet.Tower, itemKey);
        string towerName = $"{data.Name}Tower";
        Util.Log($"OnEndDrag : {towerName}를 배치 성공함");
        Managers.Resource.Instantiate(towerName, go => 
        {
            go.transform.position = buildingSystem.UpdatePosition(inputPos);
            buildingSystem.AddPlacedMap(inputPos);
            buildingSystem.DragController.IsSlotDragging = false; 
            go.GetComponent<TowerControlBase>().TakeRoot(itemKey, towerName, (Tower)Gettable);
        });
    }
    #endregion
}
