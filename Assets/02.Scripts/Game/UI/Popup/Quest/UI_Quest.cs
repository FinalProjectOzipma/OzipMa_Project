using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Quest : UI_Popup
{
    [SerializeField] private GameObject QuestWindow;
    [SerializeField] private GameObject OFFDaily;
    [SerializeField] private GameObject ONDaily;
    [SerializeField] private GameObject OFFAchivement;
    [SerializeField] private GameObject OnAchivement;

    [SerializeField] private RectTransform ListFrame;


    [SerializeField] private Button TabMenuDailyBtn;
    [SerializeField] private Button TabMenuAchivementBtn;
    [SerializeField] private Button BGClose;
    [SerializeField] private Button CloseButton;

    [SerializeField] private TextMeshProUGUI TimeText;


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
        CloseButton.onClick.AddListener(OnClickBack);
        OnDailyTab();
    }

    private void OnEnable()
    {
        OnDailyTab();
    }


    private void Update()
    {
        DateTime currentTime = Managers.Game.ServerUtcNow.ToLocalTime();

        DateTime todaySixAM = new DateTime(
            currentTime.Year,
            currentTime.Month,
            currentTime.Day,
            6, 0, 0);

        if (currentTime >= todaySixAM)
            todaySixAM = todaySixAM.AddDays(1);

        TimeSpan remaining = todaySixAM - currentTime;

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
}
