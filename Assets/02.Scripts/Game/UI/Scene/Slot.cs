using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : UI_Scene, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField] private Image Icon;
    [SerializeField] private Slider StackGageBar;
    [SerializeField] private Image Selected;

    [SerializeField] private TextMeshProUGUI ObjInfo;
    [SerializeField] private TextMeshProUGUI StackText;
    [SerializeField] private TextMeshProUGUI MaxLv;

    [SerializeField] private GameObject Normal_Slot;
    [SerializeField] private GameObject Rare_Slot;
    [SerializeField] private GameObject Epic_Slot;
    [SerializeField] private GameObject Legendary_Slot;
    [SerializeField] private GameObject Myth_Slot;


    private Button button; // 이녀석은 현재 들고 있는 컴포넌트객체니깐 그냥 Get으로 불러드림

    public int Index { get; set; }
    public bool IsActive { get; private set; } = false;

    public IGettable Gettable;

    private Sprite _sprite;
    private InventoryUI inventoryUI;
    private Image _stackGage;

    // 슬롯 드래그 관련
    private GameObject PreviewTowerBrain;
    private SpriteRenderer previewRenderer;
    private BuildingSystem buildingSystem;
    private TowerControlBase curTowerController;

    public Action<bool> onSelectionChanged;  // 선택 변경 이벤트


    private int itemKey { get; set; }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SeletToggle);

        inventoryUI = Managers.UI.GetScene<InventoryUI>();
        Selected.gameObject.SetActive(false);

        buildingSystem = BuildingSystem.Instance;

        onSelectionChanged += OnSlotSelectionChanged;
    }

    private void SeletToggle()
    {
        if (inventoryUI.CurrentState != InventoryUI.STATE.SELECTABLE) return;

        UserObject userObject = Gettable as UserObject;

        if (userObject == null)
            return;

        bool isMaxLevel = userObject.Status.Level.GetValue() >= userObject.Status.MaxLevel.GetValue();
        bool isMaxStack = userObject.Status.Stack.GetValue() >= userObject.Status.MaxStack.GetValue();


        if(!isMaxStack)
        {
            Managers.UI.Notify("카드 수가 부족합니다.", false);
            return;
        }

        if(userObject.Status.Grade.GetValue() == userObject.MaxGrade.GetValue())
        {
            Managers.UI.Notify("축하합니다. 최고 승급입니다.", false);
            return;
        }

        GameObject imgObj = Selected.gameObject;
        imgObj.SetActive(!imgObj.activeSelf);
        IsActive = imgObj.activeInHierarchy;

        onSelectionChanged?.Invoke(IsActive);
        inventoryUI.isSelect = true;

        inventoryUI.CheckActive();

        int gold = Managers.Upgrade.GetLevelUpGold(userObject);

        if (IsActive)
        {


            Managers.Upgrade.OnUpgradeGold(gold);
        }
        else
        {
            Managers.Upgrade.OnUpgradeGold(-gold);
        }

    }

    private void OnSlotSelectionChanged(bool isActive)
    {
        inventoryUI.activeSlotCount += isActive ? 1 : -1;

        if (inventoryUI.activeSlotCount > 0)
            inventoryUI.OnInchenBtn();
        else
            inventoryUI.OFFInchenBtn();
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

        SelectedSlot(obj);
        _sprite = obj.Sprite;
        Icon.sprite = _sprite;
        ObjInfo.text = $"LV.{status.Level.GetValue()}\r\nEV.{status.Grade.GetValue()}";
        StackGageBar.value = (float)status.Stack.GetValue() / status.MaxStack.GetValue();
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

    #region 드래그 배치
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inventoryUI.CurrentTab != typeof(Tower)) return;
        buildingSystem.ShowBuildHighlight(!buildingSystem.IsTowerCountFull()); // 배치 구역 표시 켜기

        // 타워 Brain 생성해서 프리뷰 역할
        Vector2 inputPos = eventData.position;
        Vector3 cellWorldPos = buildingSystem.UpdatePosition(inputPos, out Vector3Int vec3Int);
        if (PreviewTowerBrain == null)
        {
            DefaultTable.Tower data = Managers.Data.GetTable<DefaultTable.Tower>(Enums.Sheet.Tower, itemKey);
            string towerName = $"{data.Name}Tower";
            Managers.Resource.Instantiate(towerName, go =>
            {
                PreviewTowerBrain = go;
                PreviewTowerBrain.transform.position = cellWorldPos;
                curTowerController = PreviewTowerBrain.GetComponent<TowerControlBase>();
                curTowerController.TakeRoot(itemKey, towerName, (Tower)Gettable);
                curTowerController.TowerStop();

                // 애널리틱스
                #region tower_installed
                Managers.Analytics.AnalyticsTowerInstalled(itemKey.ToString(), Enum.GetName(typeof(AtkType), data.AttackType), curTowerController.TowerStatus.Level.Value,
                    vec3Int.x, vec3Int.y, Managers.Player.CurrentWave);
                #endregion
            });
        }

        inventoryUI.OnSwipe();
        buildingSystem.DragController.IsSlotDragging = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (inventoryUI.CurrentTab != typeof(Tower)) return;

        // 프리뷰 타워 위치 업데이트
        Vector2 inputPos = eventData.position;
        Vector3 cellWorldPos = buildingSystem.UpdatePosition(inputPos);
        PreviewTowerBrain.transform.position = cellWorldPos;

        // 배치 가능/불가능 색상 표현
        if (previewRenderer == null)
        {
            previewRenderer = PreviewTowerBrain.GetComponent<TowerControlBase>()?.GetTowerBodyBase()?.GetMainSpriteObj()?.GetComponent<SpriteRenderer>();
        }
        if (buildingSystem.IsTowerCountFull(false) == true || buildingSystem.CanTowerBuildArea(inputPos) == false)
        {
            previewRenderer.color = Color.red;
            return;
        }
        previewRenderer.color = Color.green;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (inventoryUI.CurrentTab != typeof(Tower)) return;
        buildingSystem.HideBuildHighlight(); // 배치 구역 표시 끄기

        // 한번 실행
        inventoryUI.OnSwipe();

        Vector2 inputPos = eventData.position;
        if (buildingSystem.IsTowerCountFull(false) == true || buildingSystem.CanTowerBuildArea(inputPos) == false)
        {
            // 배치 불가능하면 드래그 취소됨
            Managers.Resource.Destroy(PreviewTowerBrain);
            buildingSystem.DragController.IsSlotDragging = false;
            return;
        }

        // 배치 
        PreviewTowerBrain.transform.position = buildingSystem.UpdatePosition(inputPos);
        buildingSystem.AddPlacedMapScreenPos(inputPos, itemKey);
        curTowerController.TowerStart(); // 작동시키기

        // 정리
        buildingSystem.DragController.IsSlotDragging = false;
        previewRenderer.color = Color.white;
        previewRenderer = null;
        PreviewTowerBrain = null;
    }
    #endregion
}
