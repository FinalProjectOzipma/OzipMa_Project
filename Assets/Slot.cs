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

    [SerializeField] private Image Icon;
    [SerializeField] private Image StackGageFill;
    [SerializeField] private Image Selected;

    [SerializeField] private TextMeshProUGUI ObjInfo;
    [SerializeField] private TextMeshProUGUI StackText;
    [SerializeField] private TextMeshProUGUI MaxLv;


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
        button = GetComponent<Button>();
        button.onClick.AddListener(SeletToggle);

        inventoryUI = Managers.UI.GetScene<InventoryUI>();
        Selected.gameObject.SetActive(false);
    }

    private void SeletToggle()
    {
        if (inventoryUI.CurrentState != InventoryUI.STATE.SELECTABLE) return;

        UserObject userObject = Gettable as UserObject;

        if (userObject == null)
            return;

        bool isMaxLevel = userObject.Status.Level.GetValue() >= userObject.Status.MaxLevel.GetValue();

        if (isMaxLevel) return;

        GameObject imgObj = Selected.gameObject;
        imgObj.SetActive(!imgObj.activeSelf);
        IsActive = imgObj.activeInHierarchy;
        inventoryUI.isSelect = true;

        inventoryUI.CheckActive();

        if (IsActive) Managers.Upgrade.OnUpgradeGold(Managers.Upgrade.LevelUPGold);
        else Managers.Upgrade.OnUpgradeGold(-Managers.Upgrade.LevelUPGold);

    }

    // 전체 선택될때 호출해야되는 메서드
    public void OnSelect()
    {
        if (inventoryUI.CurrentState != InventoryUI.STATE.SELECTABLE) return;
        IsActive = true;
        Selected.gameObject.SetActive(true);
    }

    public void DisSelect()
    {
        IsActive = false;
        Selected.gameObject.SetActive(false);
    }


    public void SetData<T>(IGettable gettable) where T : UserObject
    {
        Gettable = gettable;
        T obj = gettable.GetClassAddress<T>();
        itemKey = obj.PrimaryKey;
        var status = obj.Status;
        _sprite = obj.Sprite;
        Icon.sprite = _sprite;
        ObjInfo.text = $"LV.{status.Level.GetValue()}\r\nEV.{status.Grade.GetValue()}";
        StackGageFill.fillAmount = status.Stack.GetValue() % status.MaxStack.GetValue();
        StackText.text = $"{status.Stack.GetValue()}/{status.MaxStack.GetValue()}";

        if (status.Level.GetValue() >= status.MaxLevel.GetValue())
        {
            MaxLv.gameObject.SetActive(true);
        }
        else
        {
            MaxLv.gameObject.SetActive(false);
        }
    }

    #region 드래그 배치
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(inventoryUI.CurrentTab != typeof(Tower)) return;

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
        inventoryUI.OnSwipe();
        buildingSystem.DragController.IsSlotDragging = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (inventoryUI.CurrentTab != typeof(Tower)) return;
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
        if (inventoryUI.CurrentTab != typeof(Tower)) return;

        // 한번 실행
        inventoryUI.OnSwipe();

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
            buildingSystem.AddPlacedMap(inputPos, itemKey);
            buildingSystem.DragController.IsSlotDragging = false; 
            go.GetComponent<TowerControlBase>().TakeRoot(itemKey, towerName, (Tower)Gettable);
        });
    }
    #endregion
}
