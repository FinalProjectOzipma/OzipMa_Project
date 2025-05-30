using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : UI_Scene
{
    #region ComponentID

    [SerializeField] private GameObject UserObjects;
    [SerializeField] private GameObject OnMyUnit;
    [SerializeField] private GameObject DisMyUnit;
    [SerializeField] private GameObject OnTower;
    [SerializeField] private GameObject DisTower;

    [SerializeField] private RectTransform Contents;

    [SerializeField] private Button BackgroundButton;
    [SerializeField] private Button SwipeBtn;
    [SerializeField] private Button InchentBtn;
    [SerializeField] private Button SelectAllBtn;
    [SerializeField] private Button MyUnitTab;
    [SerializeField] private Button TowerTab;

    [SerializeField] private TextMeshProUGUI InchentText;
    [SerializeField] private TextMeshProUGUI SelectText;
    [SerializeField] private TextMeshProUGUI TextInfo;

    [SerializeField] private GameObject InchentImage;
    [SerializeField] private GameObject SelectAllImage;
    [SerializeField] private GameObject OFFInchentImage;
    [SerializeField] private GameObject DisSelectAllImage;

    [SerializeField] private Image SwipeIcon;
    [SerializeField] private Image SwipeIconoff;
    #endregion

    #region ResourceKey
    private enum ResourceKey
    {
        TabSprite,
        SelectedTabSprite,
    }
    #endregion

    #region DefaultValue

    private Vector2 _moveDistance;

    #endregion

    #region State
    public STATE CurrentState = STATE.SELECTABLE;
    public enum STATE
    {
        SELECTABLE,
        PUTABLE,
    }
    #endregion

    private Inventory data;
    private List<Slot> slots;

    public Type CurrentTab; // 현재 탭의 타입
    private List<IGettable> _currentList; // UI에 들고있는 현재 인벤토리 데이터
    private GameObject prevOn; // 이전 탭의 컴포넌트
    private GameObject prevDis; // 이전 탭의 컴포넌트

    // Animation
    private bool isMove;

    public bool IsOpen {  get; private set; }
    public bool isSelect = false;

    public int activeSlotCount = 0;

    public Queue<Action> SwipeExcute;

    private void Awake()
    {
        Init();
        SwipeExcute = new Queue<Action>();
        OFFInchenBtn();
    }



    public override void Init()
    {
        base.Init();
        data = Managers.Player.Inventory;
        slots = new List<Slot>();
        uiSeq = Util.RecyclableSequence();

        SetBind();
        // 바인딩 후 셋팅
        _moveDistance = Contents.anchoredPosition;
        prevOn = OnTower;
        prevDis = DisTower;

        OnTowerTap();
        Managers.UI.SetSceneList<InventoryUI>(this);
        Managers.Upgrade.OnChanagedUpgrade += UpdateUpgradeGold;
        TextInfo.text = $"{Managers.Upgrade.GetUpgradeGold()}";
    }

    public GameObject GetSwipeBtn()
    {
        return SwipeBtn.gameObject;
    }


    private void UpdateUpgradeGold(int gold)
    {
        TextInfo.text = $"{gold.ToString()}";
    }


    /// <summary>
    /// 바인딩
    /// </summary>
    private void SetBind()
    {
        InchentBtn.onClick.AddListener(OnClickUpgrade);
        SelectAllBtn.onClick.AddListener(OnSelectAll);
        SwipeBtn.onClick.AddListener(OnSwipe);
        BackgroundButton.onClick.AddListener(OnSwipe);

        // Tap -----------------------------------------------------------------
        MyUnitTab.onClick.AddListener(OnMyUnitTap);
        TowerTab.onClick.AddListener(OnTowerTap);
        // ---------------------------------------------------------------------
    }

    public void Refresh<T>() where T : UserObject, IGettable
    {
        slots.Clear();
        _currentList = data.GetList<T>();
        CurrentTab = typeof(T);

        Transform trans = UserObjects.transform; // 부모 객체 얻어오기

        int cnt = 0;
        if (_currentList != null)
        {
            for (int i = 0; i < _currentList.Count; i++)
            {
                try
                {
                    SlotActive<T>(trans, trans.GetChild(i).gameObject, i);
                    cnt++;
                }
                catch (Exception)
                {
                    Managers.Resource.LoadAssetAsync<GameObject>("Slot", (go) =>
                    {
                        if (go == null) return;

                        GameObject slotGo = Managers.Resource.Instantiate(go);
                        slotGo.transform.SetParent(trans);
                        slotGo.transform.localScale = new Vector3(1f, 1f, 1f);
                        SlotActive<T>(trans, slotGo, i);
                        cnt++;
                    });
                }
            }
        }

        while (cnt < trans.childCount) // 만약 이전에 슬롯이 필요없는 상황이면 비활성화
        {
            trans.GetChild(cnt).GetComponent<Slot>().DisSelect();
            trans.GetChild(cnt++).gameObject.SetActive(false);
        }
    }

    private void SlotActive<T>(Transform parent, GameObject slotGo, int index) where T : UserObject, IGettable
    {
        Slot slot = slotGo.GetOrAddComponent<Slot>();
        slot.Index = index;
        slotGo.SetActive(true);

        slot.DisSelect();
        activeSlotCount = 0;
        OFFInchenBtn();
        IsSelectTrue();
        isSelect = false;
        slots.Add(slot);
        slot.SetData<T>(_currentList[slot.Index]);
    }

    private void OnSelectAll()
    {
        bool isButton = false;

        if (isButton) return;

        isButton = true;

        if (_currentList == null)
            return;
        Managers.Audio.PlaySFX(SFXClipName.ButtonClick);

        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(SelectAllImage.transform.DOScale(0.9f, 0.1f));
        uiSeq.Join(SelectText.transform.DOScale(0.9f, 0.1f));
        uiSeq.Append(SelectAllImage.transform.DOScale(1.1f, 0.1f));
        uiSeq.Join(SelectText.transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(SelectAllImage.transform.DOScale(1.0f, 0.1f));
        uiSeq.Join(SelectText.transform.DOScale(1.0f, 0.1f));

        uiSeq.Play();

        //bool isSelected = false;

        OnSelectfor();


        bool isMaxStack = AllMaxStack();
        bool isMaxGrade = AllMaxGrade();


        if (!isSelect && isMaxStack && !isMaxGrade)
        {
            IsSelectFalse();
        }
        else
        {
            IsSelectTrue();
        }

        isSelect = !isSelect;
        isButton = false;

    }

    public void OnSelectfor()
    {
        for (int i = 0; i < _currentList.Count; i++)
        {
            UserObject userObject = slots[i].Gettable as UserObject;
            int gold = Managers.Upgrade.GetLevelUpGold(userObject);

            if (!isSelect)
            {
                if (slots[i].IsActive || !IsStack(_currentList[i]) || IsGrade(_currentList[i])) continue;

                slots[i].OnSelect();
                slots[i].onSelectionChanged?.Invoke(true);

                Managers.Upgrade.OnUpgradeGold(gold);
            }
            else
            {
                if (!slots[i].IsActive || !IsStack(_currentList[i]) || IsGrade(_currentList[i])) continue;

                slots[i].DisSelect();
                slots[i].onSelectionChanged?.Invoke(false);
                Managers.Upgrade.OnUpgradeGold(-gold);

            }
        }
    }



    public void IsSelectFalse()
    {
        SelectAllImage.SetActive(false);
        DisSelectAllImage.SetActive(true);
        SelectText.color = Color.white;
        SelectText.text = "일괄 해제";
    }

    public void IsSelectTrue()
    {
        SelectAllImage.SetActive(true);
        DisSelectAllImage.SetActive(false);
        SelectText.color = Color.black;
        SelectText.text = "일괄 선택";
    }

    public void OFFInchenBtn()
    {
        InchentBtn.interactable = false;
        InchentImage.SetActive(false);
        OFFInchentImage.SetActive(true);
    }

    public void OnInchenBtn()
    {
        InchentBtn.interactable = true;
        InchentImage.SetActive(true);
        OFFInchentImage.SetActive(false);
    }

    public void CheckActive()
    {
        for (int i = 0; i < _currentList.Count; i++)
        {
            if (!IsStack(_currentList[i]) || IsGrade(_currentList[i])) continue;

            if (!slots[i].IsActive)
            {
                isSelect = false;
                IsSelectTrue();
                return;
            }
            else
            {
                isSelect = true;
                IsSelectFalse();
            }
        }
    }


    // 현재리스트에 만렙이 없으면 true 있으면 false
    private bool AllMaxCheck()
    {
        int AllMax = 0;

        for (int i = 0; i < _currentList.Count; i++)
        {
            if (IsMaxLevel(_currentList[i])) continue;
            AllMax++;
        }

        return AllMax == 0;
    }

    // 현재리스트에 스택을 충족하면 true 아니면 false
    private bool AllMaxStack()
    {
        int AllMax = 0;

        for (int i = 0; i < _currentList.Count; i++)
        {
            if (!IsStack(_currentList[i]) || IsGrade(_currentList[i])) continue;
            AllMax++;
        }

        return AllMax != 0;
    }


    private bool AllMaxGrade()
    {
        int AllMax = 0;

        for (int i = 0; i < _currentList.Count; i++)
        {
            if (IsGrade(_currentList[i])) continue;
            AllMax++;
        }

        return AllMax == 0;
    }


    private void OnMyUnitTap()
    {
        CurrentTab = typeof(MyUnit);
        ToggleTab(OnMyUnit, DisMyUnit);
        Refresh<MyUnit>();
        OFFInchenBtn();
        Managers.Upgrade.RefresgUpgradeGold();
        isSelect = false;
        SelectAllImage.SetActive(true);
        DisSelectAllImage.SetActive(false);
        SelectText.color = Color.black;
        SelectText.text = "일괄 선택";
    }
    private void OnTowerTap()
    {
        CurrentTab = typeof(Tower);
        ToggleTab(OnTower, DisTower);
        Refresh<Tower>();
        OFFInchenBtn();
        Managers.Upgrade.RefresgUpgradeGold();
        isSelect = false;
        SelectAllImage.SetActive(true);
        DisSelectAllImage.SetActive(false);
        SelectText.color = Color.black;
        SelectText.text = "일괄 선택";
    }


    public void OnSwipe()
    {
        if (CurrentTab == typeof(MyUnit))
        {
            Refresh<MyUnit>();
        }
        else
        {
            Refresh<Tower>();
        }

        Managers.Upgrade.RefresgUpgradeGold();

        OnAnimation();
    }

    private void Update()
    {
        if (!isMove && SwipeExcute.Count > 0)
            SwipeExcute.Dequeue()?.Invoke();
    }

    private void OnAnimation()
    {
        RectTransform movable = Contents;

        if (isMove)
        {
            SwipeExcute.Enqueue(OnSwipe);
        }
        else
        {
            isMove = true;

            if (!IsOpen)
            {
                IsOpen = true;
                gameObject.SetActive(true);
                CurrentState = STATE.SELECTABLE;
                BackgroundButton.gameObject.SetActive(true);
                Managers.UI.GetScene<UI_Main>().OnManagerMenu();
                Managers.UI.GetScene<UI_QuestRepeat>()?.gameObject.SetActive(false);
                movable.transform.DOLocalMoveY(movable.localPosition.y - _moveDistance.y + 180.0f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    isMove = false;
                    SwipeIcon.gameObject.SetActive(false);
                    SwipeIconoff.gameObject.SetActive(true);
                });
            }
            else
            {
                BackgroundButton.gameObject.SetActive(false);
                Managers.UI.GetScene<UI_Main>().OFFManagerMenu();
                Managers.UI.GetScene<UI_QuestRepeat>()?.gameObject.SetActive(true);
                movable.transform.DOLocalMoveY(movable.localPosition.y + _moveDistance.y - 180.0f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    isMove = false;
                    IsOpen = false;
                    SwipeIcon.gameObject.SetActive(true);
                    SwipeIconoff.gameObject.SetActive(false);
                });
            }
        }
    }
    private void ToggleTab(GameObject changeOn, GameObject changeDis)
    {
        if (prevOn)
        {
            prevOn.SetActive(false);
            prevDis.SetActive(true);
        }

        prevOn = changeOn;
        prevDis = changeDis;
        changeOn.SetActive(true);
        changeDis.SetActive(false);
    }

    private void OnClickUpgrade()
    {
        bool isButton = false;

        if (isButton) return;

        isButton = true;

        Managers.Audio.PlaySFX(SFXClipName.ButtonClick);

        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(InchentImage.transform.DOScale(0.9f, 0.1f));
        uiSeq.Join(InchentText.transform.DOScale(0.9f, 0.1f));
        uiSeq.Append(InchentImage.transform.DOScale(1.1f, 0.1f));
        uiSeq.Join(InchentText.transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(InchentImage.transform.DOScale(1.0f, 0.1f));
        uiSeq.Join(InchentText.transform.DOScale(1.0f, 0.1f));

        uiSeq.Play();


        if (CurrentTab == typeof(MyUnit))
        {
            LevelUpUnits<MyUnit>(Managers.Upgrade.LevelUpMyUnit);
        }
        else if (CurrentTab == typeof(Tower))
        {
            LevelUpUnits<Tower>(Managers.Upgrade.LevelUpTower);
        }

        isButton = false;
    }

    private void LevelUpUnits<T>(Action<T> levelUpAction) where T : UserObject, IGettable
    {
        bool isAnySelected = false;
        List<T> updateList = new();


        for (int i = 0; i < _currentList.Count; i++)
        {
            if (i >= slots.Count)
            {
                Util.Log("슬롯과 리스트 개수가 다릅니다.");
                return;
            }

            if (!slots[i].IsActive)
                continue;

            var select = _currentList[i] as T;

            if (select == null)
            {
                Managers.UI.Notify("널이라서 안됩니다.", false);
                return;
            }
            else if (!IsStack(select))
            {
                Managers.UI.Notify("카드 수가 부족합니다.", false);
                return;
            }
            else if (IsGrade(select))
            {
                Managers.UI.Notify("최대 승급입니다.");
                return;
            }
            else updateList.Add(select);
        }

        // 골드 확인, 등급별 강화골드 달라지면 수정해야함
        if (Managers.Player.GetGold() < Managers.Upgrade.TotalUpgradeGold)
        {
            Managers.UI.Notify("골드가 부족합니다.", false);
            RefreshUpgradeUI();
            IsSelectTrue();
            Refresh<T>();
            return;
        }
        else if (updateList.Count != 0)
        {
            for (int i = 0; i < updateList.Count; i++)
            {
                levelUpAction(updateList[i]);
                isAnySelected = true;
            }

            Managers.Audio.PlaySFX(SFXClipName.PowerUp);
        }

        if (!isAnySelected)
        {
            Managers.UI.Notify("슬롯을 선택하세요.", false);
            RefreshUpgradeUI();
            Refresh<T>();
            return;
        }


        RefreshUpgradeUI();
        Refresh<T>();
    }

    private void RefreshUpgradeUI()
    {
        Managers.Upgrade.RefresgUpgradeGold();
        isSelect = false;
        IsSelectTrue();
    }

    public bool IsMaxLevel(IGettable gettable)
    {
        var max = gettable as UserObject;

        return max.Status.Level.GetValue() == max.Status.MaxLevel.GetValue();
    }

    public bool IsStack(IGettable gettable)
    {
        var maxStack = gettable as UserObject;
        return maxStack.Status.Stack.GetValue() >= maxStack.Status.MaxStack.GetValue();
    }

    public bool IsGrade(IGettable gettable)
    {
        var maxGrade = gettable as UserObject;
        return maxGrade.Status.Grade.GetValue() == maxGrade.MaxGrade.GetValue();
    }
}
