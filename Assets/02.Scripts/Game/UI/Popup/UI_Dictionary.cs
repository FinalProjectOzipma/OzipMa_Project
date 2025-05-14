using DefaultTable;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dictionary : UI_Popup
{
    [SerializeField] private Button MyUnitBtn;
    [SerializeField] private Button TowerBtn;
    //[SerializeField] private Button BackButton;

    [SerializeField] private GameObject UIDictionary;
    [SerializeField] private GameObject MyUnitDisSelected;
    [SerializeField] private GameObject MyUnitSelected;
    [SerializeField] private GameObject TowerDisSelected;
    [SerializeField] private GameObject TowerSelected;
    [SerializeField] private RectTransform Content;



    private Type CurrentTab;
    private Inventory data;
    private List<DSlot> slots;

    private List<IGettable> _currenInventList; // UI에 들고있는 현재 인벤토리 데이터
    private GameObject prevOn; // 이전 탭의 컴포넌트
    private GameObject prevDis; // 이전 탭의 컴포넌트

    public List<Tower> DefaultTowerInfos;
    public List<MyUnit> DefaultMyUnitInfos;


    // Animation
    private bool isMove;
    private bool isOpen;
    public bool isSelect = false;

    public Queue<Action> SwipeExcute;


    private void Awake()
    {
        DefaultTowerInfos = new List<Tower>();
        DefaultMyUnitInfos = new List<MyUnit>();
        data = Managers.Player.Inventory;
        slots = new List<DSlot>();
        DefaultData();
        OnMyUnitTap();
    }

    private void OnEnable()
    {
        OnMyUnitTap();
    }

    private void Start()
    {
        MyUnitBtn.onClick.AddListener(OnMyUnitTap);
        TowerBtn.onClick.AddListener(OnTowerTap);
        //BackButton.onClick.AddListener(OnClickBackButton);
    }


    public void Refresh<T>() where T : UserObject, IGettable
    {
        slots.Clear();
        _currenInventList = data.GetList<T>();
        List<T> Dlist = DGetList<T>();
        CurrentTab = typeof(T);
        Transform trans = Content.transform; // 부모 객체 얻어오기

        for (int i = 0; i < Dlist.Count; i++)
        {
            try
            {
                SlotActive<T>(trans, trans.GetChild(i).gameObject, i, Dlist);
            }
            catch (Exception)
            {
                Managers.Resource.LoadAssetAsync<GameObject>("DSlot", (go) =>
                {
                    if (go == null) return;

                    GameObject slotGo = Managers.Resource.Instantiate(go);
                    slotGo.transform.SetParent(trans);
                    slotGo.transform.localScale = new Vector3(1f, 1f, 1f);
                    SlotActive<T>(trans, slotGo, i, Dlist);
                });
            }
        }
    }

    private void OnMyUnitTap()
    {
        CurrentTab = typeof(MyUnit);
        ToggleTab(MyUnitSelected, MyUnitDisSelected);
        Refresh<MyUnit>();
    }

    private void OnTowerTap()
    {
        CurrentTab = typeof(Tower);
        ToggleTab(TowerSelected, TowerDisSelected);
        Refresh<Tower>();
    }

    private void SlotActive<T>(Transform parent, GameObject slotGo, int index, List<T> list) where T : UserObject, IGettable
    {
        DSlot slot = slotGo.GetOrAddComponent<DSlot>();
        slot.Index = index;
        slotGo.SetActive(true);
        slots.Add(slot);

        for(int i = 0; i < list.Count; i++)
        {
            T item = list[i];
            int itemKey = (item as UserObject).PrimaryKey;

            bool exists = _currenInventList.Any(x => (x as UserObject).PrimaryKey == itemKey);

            if (exists)
            {
                slot.Screen.SetActive(false);
                slot.SetData<T>(_currenInventList[slot.Index]);
            }
            else
            {
                slot.Screen.SetActive(true);
                slot.SetData<T>(list[slot.Index]);
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



    private List<T> DGetList<T>() where T : UserObject,IGettable
    {
        if (typeof(T) == typeof(MyUnit))
        {
            return DefaultMyUnitInfos as List<T>;
        }
        else if (typeof(T) == typeof(Tower))
        {
            return DefaultTowerInfos as List<T>;
        }
        else
        {
            return null;
        }
    }


    private void DefaultData()
    {
        List<DefaultTable.Tower> Towers = Util.TableConverter<DefaultTable.Tower>(Managers.Data.Datas[Enums.Sheet.Tower]);
        List<DefaultTable.MyUnit> MyUnits = Util.TableConverter<DefaultTable.MyUnit>(Managers.Data.Datas[Enums.Sheet.MyUnit]);
        
        for(int i = 0; i < Towers.Count; i++)
        {
            Managers.Resource.LoadAssetAsync<GameObject>($"{Towers[i].Name}Tower", original =>
            {
                Tower tower = new Tower();
                tower.Init(i, original.GetComponent<TowerControlBase>().Preview);
                DefaultTowerInfos.Add(tower);
            });
        }

        for (int i = 0; i < MyUnits.Count; i++)
        {
            Managers.Resource.LoadAssetAsync<GameObject>($"{MyUnits[i].Name}_Brain", original =>
            {
                MyUnit unit = new MyUnit();
                unit.Init(i, original.GetComponent<MyUnitController>().sprite);
                DefaultMyUnitInfos.Add(unit);
            });
        }

    }

    //public void OnClickBackButton()
    //{
    //    ClosePopupUI();
    //}
}
