using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : UI_Scene
{
    #region ComponentID
    private enum GameObjects
    {
        UserObjects,

        // Tab
        OnMyUnit,
        DisMyUnit,
        OnTower,
        DisTower,
    }

    private enum RectTransforms
    {
        Contents,
    }

    private enum Buttons
    {
        SwipeBtn,
        InchentBtn,
        SelectAllBtn,
        PutBtn,

        MyUnitTab,
        TowerTab,
    }

    private enum Texts
    {
        InchentText,
        SelectText,
        PutText,
        TextInfo
    }

    private enum Images
    {
        InchentImage,
        SelectAllImage,
        PutImage,
        SwipeIcon,
    }
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

    private List<IGettable> _currentList; // UI에 들고있는 현재 인벤토리 데이터
    private Type _currentTab; // 현재 탭의 타입
    private GameObject prevOn; // 이전 탭의 컴포넌트
    private GameObject prevDis; // 이전 탭의 컴포넌트

    // Animation
    private bool isMove;
    private bool isOpen;
    private bool isBatch = false;
    public bool isSelect = false;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        data = Managers.Player.Inventory;
        slots = new List<Slot>();
        uiSeq = Util.RecyclableSequence();

        SetBind();
        // 바인딩 후 셋팅
        _moveDistance = GetRect((int)RectTransforms.Contents).anchoredPosition;
        prevOn = GetObject((int)GameObjects.OnTower);
        prevDis = GetObject((int)GameObjects.DisTower);

        OnTowerTap();
        Managers.UI.SetSceneList<InventoryUI>(this);
        Managers.Upgrade.OnChanagedUpgrade += UpdateUpgradeGold;
        Get<TextMeshProUGUI>((int)Texts.TextInfo).text = $"강화 비용 : {Managers.Upgrade.GetUpgradeGold()}";
    }


    private void UpdateUpgradeGold(int gold)
    {
        Get<TextMeshProUGUI>((int)Texts.TextInfo).text = $"강화 비용 : {gold.ToString()}" ;
    }


    /// <summary>
    /// 바인딩
    /// </summary>
    private void SetBind()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<RectTransform>(typeof(RectTransforms));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.InchentBtn).onClick.AddListener(OnClickUpgrade);
        GetButton((int)Buttons.SelectAllBtn).onClick.AddListener(OnSelectAll);
        GetButton((int)Buttons.SwipeBtn).onClick.AddListener(OnSwipe);

        // Tap -----------------------------------------------------------------
        GetButton((int)Buttons.MyUnitTab).onClick.AddListener(OnMyUnitTap);
        GetButton((int)Buttons.TowerTab).onClick.AddListener(OnTowerTap);
        GetButton((int)Buttons.PutBtn).onClick.AddListener(OnPut);
        // ---------------------------------------------------------------------
    }

    public void Refresh<T>() where T : UserObject, IGettable
    {
        slots.Clear();
        _currentList = data.GetList<T>();
        _currentTab = typeof(T);

        Transform trans = GetObject((int)GameObjects.UserObjects).transform; // 부모 객체 얻어오기

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
        slots.Add(slot);
        slot.SetData<T>(_currentList[slot.Index]);
    }

    private void OnSelectAll()
    {
        if (_currentList == null)
            return;
        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);

        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(Get<Image>((int)Images.SelectAllImage).transform.DOScale(0.9f, 0.1f));
        uiSeq.Join(Get<TextMeshProUGUI>((int)Texts.SelectText).transform.DOScale(0.9f, 0.1f));
        uiSeq.Append(Get<Image>((int)Images.SelectAllImage).transform.DOScale(1.1f, 0.1f));
        uiSeq.Join(Get<TextMeshProUGUI>((int)Texts.SelectText).transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(Get<Image>((int)Images.SelectAllImage).transform.DOScale(1.0f, 0.1f));
        uiSeq.Join(Get<TextMeshProUGUI>((int)Texts.SelectText).transform.DOScale(1.0f, 0.1f));

        uiSeq.Play();

        //bool isSelected = false;


        for (int i = 0; i < _currentList.Count; i++)
        {
            if (!isSelect)
            {
                slots[i].OnSelect();
            }   
            else
            {
                if (slots[i].IsActive)
                {
                    slots[i].DisSelect();

                    if (IsMaxLevel(_currentList[i])) continue;

                    Managers.Upgrade.OnUpgradeGold(-Managers.Upgrade.LevelUPGold);
                }
            }
        }

        isSelect = !isSelect;
    }

    private bool CheckActive()
    {
        for (int i = 0; i < _currentList.Count; i++)
        {
            if (!slots[i].IsActive) return false;
        }

        return true;
    }

    private void OnMyUnitTap()
    {
        _currentTab = typeof(MyUnit);
        ToggleTab(GetObject((int)GameObjects.OnMyUnit), GetObject((int)GameObjects.DisMyUnit));
        GetButton((int)Buttons.PutBtn).gameObject.SetActive(false);
        Refresh<MyUnit>();
        Managers.Upgrade.RefresgUpgradeGold();
        isSelect = false;
    }
    private void OnTowerTap()
    {
        _currentTab = typeof(Tower);
        ToggleTab(GetObject((int)GameObjects.OnTower), GetObject((int)GameObjects.DisTower));
        GetButton((int)Buttons.PutBtn).gameObject.SetActive(true);
        Refresh<Tower>();
        Managers.Upgrade.RefresgUpgradeGold();
        isSelect = false;
    }


    public void OnSwipe()
    {
        if (_currentTab == typeof(MyUnit))
        {
            Refresh<MyUnit>();
        }
        else
        {
            Refresh<Tower>();
        }

        Managers.Upgrade.RefresgUpgradeGold();  

        OnAnimation();

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);
    }

    private void OnAnimation()
    {
        RectTransform movable = GetRect((int)RectTransforms.Contents);
        if (!isMove)
        {
            Managers.UI.GetScene<UI_Main>().OffButton();
            isMove = true;
            if (!isOpen)
            {
                isOpen = true;
                gameObject.SetActive(true);
                Get<Image>((int)Images.PutImage).color = Color.white;
                CurrentState = STATE.SELECTABLE;
                movable.transform.DOLocalMoveY(movable.localPosition.y - _moveDistance.y, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    isMove = false;
                    GetImage((int)Images.SwipeIcon).transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90.0f));
                });
            }
            else
            {
                movable.transform.DOLocalMoveY(movable.localPosition.y + _moveDistance.y, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    isMove = false;
                    isOpen = false;
                    Managers.UI.GetScene<UI_Main>().OnButton();
                    GetImage((int)Images.SwipeIcon).transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90.0f));
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

    private void OnPut()
    {
        if (isBatch) return;

        isBatch = true;
        Refresh<Tower>();

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);

        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(Get<Image>((int)Images.PutImage).transform.DOScale(0.9f, 0.1f));
        uiSeq.Join(Get<TextMeshProUGUI>((int)Texts.PutText).transform.DOScale(0.9f, 0.1f));
        uiSeq.Append(Get<Image>((int)Images.PutImage).transform.DOScale(1.1f, 0.1f));
        uiSeq.Join(Get<TextMeshProUGUI>((int)Texts.PutText).transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(Get<Image>((int)Images.PutImage).transform.DOScale(1.0f, 0.1f));
        uiSeq.Join(Get<TextMeshProUGUI>((int)Texts.PutText).transform.DOScale(1.0f, 0.1f));

        uiSeq.Play();

        if (CurrentState == STATE.PUTABLE)
        {
            Get<Image>((int)Images.PutImage).color = Color.white;
            CurrentState = STATE.SELECTABLE;
            isBatch = false;
        }
        else if (CurrentState == STATE.SELECTABLE)
        {
            // 배치모드 ON
            Get<Image>((int)Images.PutImage).color = Color.gray;
            CurrentState = STATE.PUTABLE;
            isBatch = false;
        }
    }


    private void OnClickUpgrade()
    {
        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);

        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(Get<Image>((int)Images.InchentImage).transform.DOScale(0.9f, 0.1f));
        uiSeq.Join(Get<TextMeshProUGUI>((int)Texts.InchentText).transform.DOScale(0.9f, 0.1f));
        uiSeq.Append(Get<Image>((int)Images.InchentImage).transform.DOScale(1.1f, 0.1f));
        uiSeq.Join(Get<TextMeshProUGUI>((int)Texts.InchentText).transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(Get<Image>((int)Images.InchentImage).transform.DOScale(1.0f, 0.1f));
        uiSeq.Join(Get<TextMeshProUGUI>((int)Texts.InchentText).transform.DOScale(1.0f, 0.1f));

        uiSeq.Play();


        if (_currentTab == typeof(MyUnit))
        {
            LevelUpUnits<MyUnit>(Managers.Upgrade.LevelUpMyUnit);
        }
        else if (_currentTab == typeof(Tower))
        {
            LevelUpUnits<Tower>(Managers.Upgrade.LevelUpTower);
        }
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

            // select가 널이 아니거나 MaxLevel이 아니면 업데이트리스트 추가
            if(select != null && select.Status.Level != select.Status.MaxLevel)
            {
                updateList.Add(select);
            }
        }

        // 골드 확인, 등급별 강화골드 달라지면 수정해야함
        if (Managers.Player.GetGold() < Managers.Upgrade.LevelUPGold * updateList.Count)
        {
            Managers.UI.ShowPopupUI<UI_Popup>("GoldAlarmPopup");
            Managers.Upgrade.RefresgUpgradeGold();
            Refresh<T>();
            return;
        }
        else
        {
            for (int i = 0; i < updateList.Count; i++)
            {
                levelUpAction(updateList[i]);
                isAnySelected = true;
            }
        }

        if (!isAnySelected)
        {
            Managers.UI.ShowPopupUI<UI_Alarm>("InchentPopup");
            return;
        }

        Managers.Upgrade.RefresgUpgradeGold();
        isSelect = false;
        Refresh<T>();
    }

    public bool IsMaxLevel(IGettable gettable)
    {
        var max = gettable as UserObject;

        return max.Status.Level.GetValue() == max.Status.MaxLevel.GetValue();
    }

    public void TextMaxLevel()
    {
        Get<TextMeshProUGUI>((int)Texts.TextInfo).text = "최고 레벨 입니다.";
    }
}
