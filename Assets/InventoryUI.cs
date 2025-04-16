using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class InventoryUI : UI_Scene
{
    #region ComponentID
    private enum InventoryObj
    {
        UserObjects,
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

    private enum Images
    {
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

    private Inventory data; 
    private List<Slot> slots = new(); // 현재 슬롯

    private List<IGettable> _currentList; // UI에 들고있는 현재 인벤토리 데이터
    private Type _currentTab; // 현재 탭의 타입
    private Image _prevImage; // 이전 탭의 컴포넌트

    // Resource Key
    

    private void Awake()
    {
        Init();
        Managers.Resource.LoadResourceLoacationAsync(nameof(GameScene), Init);
    }

    public override void Init()
    {
        base.Init();
        data = Managers.Player.Inventory;

        // 이건 테스트용-------------------
        for (int i = 0; i < 20; i++)
        {
            Tower tower = new Tower();
            Managers.Resource.LoadAssetAsync<Sprite>("SprSquare", (sprite) => { tower.Init(20, sprite); });
            data.Add<Tower>(tower);
        }
        //---------------------------------

        slots = new List<Slot>(); // UI측면
        SetBind();
        _prevImage = GetImage((int)Images.TowerTab);
        OnTowerTap();
    }

    /// <summary>
    /// 
    /// </summary>
    private void SetBind()
    {
        Bind<GameObject>(typeof(InventoryObj));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.SelectAllBtn).onClick.AddListener(OnSelectAll);
        GetButton((int)Buttons.SwipeBtn).onClick.AddListener(OnSwipe);

        // Tap -----------------------------------------------------------------
        GetButton((int)Buttons.MyUnitTab).onClick.AddListener(OnMyUnitTap);
        GetButton((int)Buttons.TowerTab).onClick.AddListener(OnTowerTap);
        // ---------------------------------------------------------------------
    }

    public void Refresh<T>() where T : UserObject, IGettable
    {
        slots.Clear();
        _currentList = data.GetList<T>();
        _currentTab = typeof(T);

        Transform trans = GetObject((int)InventoryObj.UserObjects).transform; // 부모 객체 얻어오기

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
            trans.GetChild(cnt++).gameObject.SetActive(false);
        }
    }

    private void SlotActive<T>(Transform parent ,GameObject slotGo) where T : UserObject, IGettable
    {
        Slot slot = slotGo.GetOrAddComponent<Slot>(); 
        slotGo.SetActive(true);

        slots.Add(slot);
        slot.SetData<T>(_currentList[slot.Index]);
    }

    private void OnSelectAll()
    {
        if (_currentList == null)
            return;

        for (int i = 0; i < _currentList.Count; i++)
        {
            slots[i].OnSelect();
        }
    }   

    private void OnMyUnitTap()
    {
        if (_prevImage == null)
            _prevImage = (GetImage((int)Images.MyUnitTab));

        ChangeTap<MyUnit>(GetImage((int)Images.MyUnitTab));
    }

    private void OnTowerTap()
    {
        ChangeTap<Tower>(GetImage((int)Images.TowerTab));
    }

    private void OnSwipe()
    {
        // DoTween

        if (_currentList == null)
        {
            Refresh<MyUnit>();
            return;
        }

        if (_currentTab == typeof(MyUnit))
        {
            Refresh<MyUnit>();
        }
        else
        {
            Refresh<Tower>();
        }
    }

    private void ChangeTap<T>(Image changeImg) where T : UserObject, IGettable
    {
        Managers.Resource.LoadAssetAsync<Sprite>(ResourceKey.TabSprite.ToString(), (spr) =>
        {
            if (_prevImage != null)
                _prevImage.sprite = spr;
        });

        Managers.Resource.LoadAssetAsync<Sprite>(ResourceKey.SelectedTabSprite.ToString(), (spr) =>
        {
            changeImg.sprite = spr;
            Refresh<T>();
            _prevImage = changeImg;
        });
    }

}
