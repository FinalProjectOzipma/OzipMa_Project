using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Dictionary : UI_Popup
{
    [SerializeField] private Button MyUnitBtn;
    [SerializeField] private Button TowerBtn;
    [SerializeField] private Button BGClose;
    //[SerializeField] private Button BackButton;

    [SerializeField] public GameObject UIDictionary;
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
        AnimePopup(UIDictionary);
    }

    private void Start()
    {
        MyUnitBtn.onClick.AddListener(OnMyUnitTap);
        TowerBtn.onClick.AddListener(OnTowerTap);
        BGClose.gameObject.BindEvent(OnClickClose);
        AnimePopup(UIDictionary);
    }

    /// <summary>
    /// 도감에 슬롯 갱신해주는 메서드
    /// </summary>

    public void Refresh<T>() where T : UserObject, IGettable
    {
        slots.Clear();
        _currenInventList = data.GetList<T>();
        List<T> Dlist = DGetList<T>();
        CurrentTab = typeof(T);
        Transform trans = Content.transform; // 부모 객체 얻어오기

        int cnt = 0;
        for (int i = 0; i < Dlist.Count; i++)
        {
            try
            {
                SlotActive<T>(trans.GetChild(i).gameObject, i, Dlist);
                cnt++;
            }
            catch (Exception)
            {
                Managers.Resource.LoadAssetAsync<GameObject>("DSlot", (go) =>
                {
                    if (go == null) return;

                    GameObject slotGo = Managers.Resource.Instantiate(go);
                    slotGo.transform.SetParent(trans);
                    slotGo.transform.localScale = new Vector3(1f, 1f, 1f);
                    SlotActive<T>(slotGo, i, Dlist);
                    cnt++;
                });
            }

        }

        while (cnt < trans.childCount) // 만약 이전에 슬롯이 필요없는 상황이면 비활성화
        {
            trans.GetChild(cnt++).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 도감을 유닛으로 슬롯 채움
    /// </summary>
    private void OnMyUnitTap()
    {
        CurrentTab = typeof(MyUnit);
        ToggleTab(MyUnitSelected, MyUnitDisSelected);
        Refresh<MyUnit>();
    }

    /// <summary>
    /// 도감을 타워로 슬롯 채움
    /// </summary>
    private void OnTowerTap()
    {
        CurrentTab = typeof(Tower);
        ToggleTab(TowerSelected, TowerDisSelected);
        Refresh<Tower>();
    }


    /// <summary>
    /// 슬롯에 정보를 채워주고 와 인벤토리에 유닛이 있는지 여부 확인
    /// </summary>
    private void SlotActive<T>(GameObject slotGo, int index, List<T> list) where T : UserObject, IGettable
    {
        DSlot slot = slotGo.GetOrAddComponent<DSlot>();
        slot.Index = index;
        slotGo.SetActive(true);
        slots.Add(slot);

        bool exists = false;
        T matchedItem = default;

        UserObject currentItem = list[index] as UserObject;
        if (currentItem == null)
            return;

        foreach (var x in _currenInventList)
        {
            UserObject userObj = x as UserObject;
            if (userObj != null && userObj.PrimaryKey == currentItem.PrimaryKey)
            {
                exists = true;
                matchedItem = x as T;
                break;
            }
        }

        if (exists)
        {
            slot.Screen.SetActive(false);
            slot.Button.interactable = true;
            slot.SetData<T>(matchedItem);
        }
        else
        {
            slot.Screen.SetActive(true);
            slot.Button.interactable = false;
            slot.SetData<T>(list[index]);
        }

    }

    /// <summary>
    /// 토글탭 전환
    /// </summary>
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


    /// <summary>
    /// 타입에 따라 디폴트 유닛과 타워 리스트를 반환
    /// </summary>
    private List<T> DGetList<T>() where T : UserObject, IGettable
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

    /// <summary>
    /// 디폴트 데이터를 받아와 도감에 있는 유닛과 타워 리스트에 채워줌
    /// </summary>

    private void DefaultData()
    {
        List<DefaultTable.Tower> Towers = Util.TableConverter<DefaultTable.Tower>(Managers.Data.Datas[Enums.Sheet.Tower]);
        List<DefaultTable.MyUnit> MyUnits = Util.TableConverter<DefaultTable.MyUnit>(Managers.Data.Datas[Enums.Sheet.MyUnit]);

        for (int i = 0; i < Towers.Count; i++)
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

    /// <summary>
    /// 메인씬 버튼에서 꺼주는 메서드
    /// </summary>
    public void OnClickClose(PointerEventData data)
    {
        Managers.UI.GetScene<UI_Main>().OnClickDictionary(data);
    }

    public bool IsSlotOpen()
    {
        foreach (DSlot item in slots)
        {
            if (item.IsSlotOpen == true)
            {
                return true;
            }
        }
        return false;
    }
}
