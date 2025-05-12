using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_OFFLinePopup : UI_Popup
{
    [SerializeField] private Button CheckButton;
    [SerializeField] private Button DoubleButton;

    [SerializeField] TextMeshProUGUI TimerText;
    [SerializeField] TextMeshProUGUI RewordGoldText;
    [SerializeField] TextMeshProUGUI RewordZamText;

    [SerializeField] private Image CheckImage;
    [SerializeField] private Image DoubleImage;

    [SerializeField] private GameObject UIOffLine;


    private int baseGoldPerMinute = 1; // 1분당 1골드
    private float baseGemChance = 0.02f;  // 1분당 2% 확률로 1잼 지급

    private DateTime rewordStartTime;
    private float elapsedMinute; // 경과되는 시간
    private int rewordGold;
    private int rewordGem;

    private bool isRewordClaimed = false;

    private void Awake()
    {
        if(!string.IsNullOrEmpty(Managers.Player.RewordStartTime))
        {
            Init();
        }
        else
        {
            this.gameObject.SetActive(false);
        }

    }

    private void Start()
    {
        int totalMinutes = Mathf.FloorToInt(elapsedMinute);
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        string formattedTime = string.Format("{0}시간 {1}분", hours, minutes);
        TimerText.text = formattedTime;
        RewordGoldText.text = rewordGold.ToString();
        RewordZamText.text = rewordGem.ToString();
    }

    public override void Init()
    {
        CheckButton.gameObject.BindEvent(OnClickReword);
        DoubleButton.gameObject.BindEvent(OnClickDoubleReword);

        rewordStartTime = DateTime.Parse(Managers.Player.RewordStartTime, null, System.Globalization.DateTimeStyles.RoundtripKind);

        TimeSpan passed = Managers.Game.ServerUtcNow - rewordStartTime; // 오프라인 경과 시간
        elapsedMinute = (float)(passed).TotalMinutes;

        GetReword();
    }

    private void OnClickReword(PointerEventData data)
    {
        if (isRewordClaimed) return;
        isRewordClaimed = true;

        Managers.Player.AddGold((long)rewordGold);
        Managers.Player.AddGem((long)rewordGem);

        HidePpoup();

    }

    private void OnClickDoubleReword(PointerEventData data)
    {
        if (isRewordClaimed) return;
        isRewordClaimed = true;

        rewordGold *= 2;
        rewordGem *= 2;
        Managers.Player.AddGold((long)rewordGold);
        Managers.Player.AddGem((long)rewordGem);

        HidePpoup();

    }

    private void GetReword()
    {
        float cappedElapsedHours = Mathf.Min(elapsedMinute, 2880f); // 최대 48시간 제한

        if (cappedElapsedHours < 1.0f)
        {
            this.gameObject.SetActive(false);
            return;
        }
        
        rewordGold =(Managers.Player.CurrentStage + 1) * baseGoldPerMinute * (int)cappedElapsedHours;
        rewordGem = Mathf.FloorToInt(cappedElapsedHours * 60 * baseGemChance);
    }

    private void HidePpoup()
    {
        AnimePopup(UIOffLine, true);

        uiSeq.Play().OnComplete(() =>
        {
            this.gameObject.SetActive(false);
            isRewordClaimed = false;
        });
    }

}
