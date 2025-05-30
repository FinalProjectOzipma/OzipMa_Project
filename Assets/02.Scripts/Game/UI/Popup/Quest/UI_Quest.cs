using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Quest : UI_Popup
{
    [SerializeField] private GameObject QuestWindow;
    [SerializeField] private GameObject OFFDaily;
    [SerializeField] private GameObject ONDaily;
    [SerializeField] private GameObject OFFAchivement;
    [SerializeField] private GameObject OnAchivement;
    [SerializeField] private GameObject DailyCheckIcon;
    [SerializeField] private GameObject AchivementCheckIcon;

    [SerializeField] private RectTransform ListFrame;


    [SerializeField] private Button TabMenuDailyBtn;
    [SerializeField] private Button TabMenuAchivementBtn;
    [SerializeField] private Button BGClose;


    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private ScrollRect Middle_ScrollRect;


    private List<UI_QuestSlot> questSlot;
    private List<QuestData> questDatas;

    private GameObject prevOn; // 이전 탭의 컴포넌트
    private GameObject prevDis; // 이전 탭의 컴포넌트



    private void Awake()
    {
        questSlot = new();
        questDatas = new();

        TabMenuDailyBtn.onClick.AddListener(OnDailyTab);
        TabMenuAchivementBtn.onClick.AddListener(OnAchivementTab);
        BGClose.onClick.AddListener(OnClickBack);
        Middle_ScrollRect.verticalNormalizedPosition = 1.0f;


        OnDailyTab();
    }

    private void OnEnable()
    {
        OnDailyTab();
        Managers.Quest.OnAnyQuestCompleted += ActiveDailyAlarm;
        Managers.Quest.OnAnyQuestCompleted += ActiveAchivementAlarm;
        Middle_ScrollRect.verticalNormalizedPosition = 1.0f;

        ActiveDailyAlarm();
        ActiveAchivementAlarm();
    }

    private void OnDisable()
    {
        Managers.Quest.OnAnyQuestCompleted -= ActiveDailyAlarm;
        Managers.Quest.OnAnyQuestCompleted -= ActiveAchivementAlarm;
    }

    private void Update()
    {
        //DateTime currentTime = DateTime.UtcNow.AddHours(9);
        DateTime currentTime = Managers.Game.ServerUtcNow.AddHours(9);

        DateTime todayMidnightUtc = new DateTime(
            currentTime.Year,
            currentTime.Month,
            currentTime.Day,
            0, 0, 0);

        if (currentTime >= todayMidnightUtc)
        {
            todayMidnightUtc = todayMidnightUtc.AddDays(1);
        }


        TimeSpan remaining = todayMidnightUtc - currentTime;

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
            remaining.Hours + remaining.Days * 24,
            remaining.Minutes,
            remaining.Seconds);

        TimeText.text = formattedTime;

    }



    public void Refresh(QuestType questType)
    {
        questSlot.Clear();

        questDatas = Managers.Quest.GetQuestList(questType);

        Transform trans = ListFrame.transform; // 부모 객체 얻어오기

        int cnt = 0;
        for (int i = 0; i < questDatas.Count; i++)
        {
            try
            {
                SlotActive(trans.GetChild(i).gameObject, i);
                cnt++;
            }
            catch (Exception)
            {
                Managers.Resource.LoadAssetAsync<GameObject>("QuestSlot", (go) =>
                {
                    if (go == null) return;

                    GameObject slotGo = Managers.Resource.Instantiate(go);
                    slotGo.transform.SetParent(trans);
                    slotGo.transform.localScale = new Vector3(1f, 1f, 1f);
                    SlotActive(slotGo, i);
                    cnt++;
                });
            }

        }

        while (cnt < trans.childCount) // 만약 이전에 슬롯이 필요없는 상황이면 비활성화
        {
            trans.GetChild(cnt++).gameObject.SetActive(false);
        }


        for (int i = 0; i < questSlot.Count; i++)
        {
            if (questSlot[i].CheckIsComplete()) questSlot[i].gameObject.transform.SetAsLastSibling();
            if (questSlot[i].questData.State == QuestState.Done) questSlot[i].gameObject.transform.SetAsFirstSibling();
        }
    }

    private void SlotActive(GameObject slotGo, int index)
    {
        UI_QuestSlot slot = slotGo.GetOrAddComponent<UI_QuestSlot>();
        slot.Index = index;
        slotGo.SetActive(true);
        questSlot.Add(slot);
        slot.SetData(questDatas[index]);
    }

    private void OnDailyTab()
    {
        ToggleTab(ONDaily, OFFDaily);
        Refresh(QuestType.Daily);
    }

    private void OnAchivementTab()
    {
        ToggleTab(OnAchivement, OFFAchivement);
        Refresh(QuestType.Achivement);
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



    public void OnClickBack()
    {
        bool isButton = false;

        if (isButton) return;
        isButton = true;

        Managers.Audio.PlaySFX(SFXClipName.ButtonClick);
        ClosePopupUI();
    }

    public void ActiveDailyAlarm()
    {
        bool show = Managers.Quest.HasAnyCompleteDaily();

        if (show)
        {
            Util.Log("현재 true다");
        }
        else
        {
            Util.Log("현재 false다");
        }

        DailyCheckIcon.SetActive(show);
    }

    public void ActiveAchivementAlarm()
    {
        bool show = Managers.Quest.HasAnyCompleteAchivement();

        if (show)
        {
            Util.Log("현재 true다");
        }
        else
        {
            Util.Log("현재 false다");
        }

        AchivementCheckIcon.SetActive(show);
    }
}
