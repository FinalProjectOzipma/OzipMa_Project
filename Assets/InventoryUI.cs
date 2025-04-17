using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private STATE _currentState = STATE.SELECTABLE;
    private enum STATE
    {
        SELECTABLE,
        PUTABLE,
    }
    #endregion

    private Inventory data; 
    private List<Slot> slots = new(); // 현재 슬롯

    private List<IGettable> _currentList; // UI에 들고있는 현재 인벤토리 데이터
    private Type _currentTab; // 현재 탭의 타입
    private GameObject _prevOn; // 이전 탭의 컴포넌트
    private GameObject _prevDis; // 이전 탭의 컴포넌트

    // Animation
    private bool isMove;
    private bool isOpen;

    private bool isSelected = false;

    private void Awake()
    {
        Managers.Resource.LoadResourceLoacationAsync(nameof(GameScene), Init);
    }

    public override void Init()
    {
        base.Init();
        data = Managers.Player.Inventory;
        slots = new List<Slot>();
        uiSeq = Util.RecyclableSequence();

        // 이건 테스트용-------------------
        for (int i = 0; i < 10; i++)
        {
            MyUnit unit = new MyUnit();
            Managers.Resource.LoadAssetAsync<Sprite>("SprSquare", (sprite) => { unit.Init(20, sprite); });
            data.Add<MyUnit>(unit);
        }

        for (int i = 0; i < 20; i++)
        {
            Tower tower = new Tower();
            Managers.Resource.LoadAssetAsync<Sprite>("SprSquare", (sprite) => { tower.Init(20, sprite); });
            data.Add<Tower>(tower);
        }

        
        //---------------------------------

        SetBind();
        // 바인딩 후 셋팅
        _moveDistance = GetRect((int)RectTransforms.Contents).anchoredPosition;
        _prevOn = GetObject((int)GameObjects.OnTower);
        _prevDis = GetObject((int)GameObjects.DisTower);

        OnTowerTap();
    }

    /// <summary>
    /// 바인딩
    /// </summary>
    private void SetBind()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<RectTransform>(typeof(RectTransforms));
        Bind<Button>(typeof(Buttons));

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
        isSelected = false;
        slots.Clear();
        _currentList = data.GetList<T>();
        _currentTab = typeof(T);

        Transform trans = GetObject((int)GameObjects.UserObjects).transform; // 부모 객체 얻어오기

        int cnt = 0;
        if(_currentList != null)
        {
            for (int i = 0; i < _currentList.Count; i++)
            {
                try
                {
                    SlotActive<T>(trans, trans.GetChild(i).gameObject);
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
                        SlotActive<T>(trans, slotGo);
                        cnt++;
                    });
                }
            }
        }
        
        while(cnt < trans.childCount) // 만약 이전에 슬롯이 필요없는 상황이면 비활성화
        {
            trans.GetChild(cnt).GetComponent<Slot>().DisSelect();
            trans.GetChild(cnt++).gameObject.SetActive(false);
        }
    }

    private void SlotActive<T>(Transform parent ,GameObject slotGo) where T : UserObject, IGettable
    {
        Slot slot = slotGo.GetOrAddComponent<Slot>(); 
        slotGo.SetActive(true);

        slot.DisSelect();
        slots.Add(slot);
        slot.SetData<T>(_currentList[slot.Index]);
    }

    private void OnSelectAll()
    {
        if (_currentList == null)
            return;

        for (int i = 0; i < _currentList.Count; i++)
        {
            if (!isSelected)
                slots[i].OnSelect();
            else
                slots[i].DisSelect();
        }

        isSelected = !isSelected;
    }   

    private void OnMyUnitTap()
    {
        ToggleTab(GetObject((int)GameObjects.OnMyUnit), GetObject((int)GameObjects.DisMyUnit));
        GetButton((int)Buttons.PutBtn).gameObject.SetActive(false);
        Refresh<MyUnit>();
    }
    private void OnTowerTap()
    {
        ToggleTab(GetObject((int)GameObjects.OnTower), GetObject((int)GameObjects.DisTower));
        GetButton((int)Buttons.PutBtn).gameObject.SetActive(true);
        Refresh<Tower>();
    }


    private void OnSwipe()
    {
        if (_currentTab == typeof(MyUnit))
        {
            Refresh<MyUnit>();
        }
        else
        {
            Refresh<Tower>();
        }

        OnAnimation();
    }

    private void OnAnimation()
    {
        RectTransform movable = GetRect((int)RectTransforms.Contents);
        if (!isMove)
        {
            isMove = true;
            if(!isOpen)
            {
                isOpen = true;
                gameObject.SetActive(true);
                movable.transform.DOLocalMoveY(movable.localPosition.y - _moveDistance.y, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    isMove = false;
                });
            }
            else
            {
                movable.transform.DOLocalMoveY(movable.localPosition.y + _moveDistance.y, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    isMove = false;
                    isOpen = false;
                });
            }
        }   
    }
    private void ToggleTab(GameObject changeOn, GameObject changeDis)
    {
        if (_prevOn)
        {
            _prevOn.SetActive(false);
            _prevDis.SetActive(true);
        }

        _prevOn = changeOn;
        _prevDis = changeDis;
        changeOn.SetActive(true);
        changeDis.SetActive(false);
    }
    
    private void OnPut()
    {
        Refresh<Tower>();
        _currentState = STATE.PUTABLE;

        // TODO::
    }
}
