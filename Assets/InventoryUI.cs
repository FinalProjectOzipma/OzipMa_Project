using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class InventoryUI : UI_Scene
{

    private enum InventoryObj
    {
        UserObjects
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

    public AssetLabelReference assetLabel;

    // 필요한 정보들
    // 탭, 버튼, 슬롯
    private Inventory data;
    private List<Slot> slots = new();

    private Transform myUnitPanel;
    private Button[] buttons;
    //private Tap tap;

    private List<IGettable> _currentList;
    private Type _currentTab;
    private Image _prevImage;

    // Resource Key
    string TabSprite = nameof(TabSprite);
    string SelectedTabSprite = nameof(SelectedTabSprite);

    private void Awake()
    {
        Init();
        // ResourceLoad
        Managers.Resource.LoadAssetAsync<GameObject>("Slot");
        Managers.Resource.LoadResourceLoacationAsync<Sprite>(assetLabel);
    }

    public override void Init()
    {
        base.Init();
        data = Managers.Player.Inventory;

        // 이건 테스트용-------------------
        for (int i = 0; i < 2; i++)
        {
            Tower tower = new Tower();
            Managers.Resource.LoadAssetAsync<Sprite>("SprSquare", (sprite) => { tower.Init(20, sprite); });
            data.Add<Tower>(tower);
        }
        //---------------------------------

        slots = new List<Slot>(); // UI측면
        Bind<GameObject>(typeof(InventoryObj));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        _prevImage = GetImage((int)Images.MyUnitTab);

        GetButton((int)Buttons.SelectAllBtn).onClick.AddListener(OnSelectAll);
        GetButton((int)Buttons.SwipeBtn).onClick.AddListener(OnSwipe);

        // Tap -----------------------------------------------------------------
        GetButton((int)Buttons.MyUnitTab).onClick.AddListener(OnMyUnitTap);
        GetButton((int)Buttons.TowerTab).onClick.AddListener(OnTowerTap);
        // ---------------------------------------------------------------------
        OnTowerTap();
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
        ChangeTap<MyUnit>(GetImage((int)Images.MyUnitTab));
    }

    private void OnTowerTap()
    {
        ChangeTap<Tower>(GetImage((int)Images.TowerTab));
    }

    private void ChangeTap<T>(Image changeImg) where T : UserObject, IGettable
    {
        Managers.Resource.LoadAssetAsync<Sprite>(TabSprite, (spr) =>
        {
            if (_prevImage != null)
                _prevImage.sprite = spr;
        });

        Managers.Resource.LoadAssetAsync<Sprite>(SelectedTabSprite, (spr) =>
        {
            changeImg.sprite = spr;
            Refresh<T>();
            _prevImage = changeImg;
        });
    }

    private void OnSwipe()
    {
        if (_currentList == null)
        {
            Refresh<MyUnit>();
            return;
        }

        if (_currentTab == typeof(MyUnit))
            Refresh<MyUnit>();
        else
            Refresh<Tower>();
    }
}
