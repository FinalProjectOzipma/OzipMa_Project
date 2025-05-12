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


    private int baseGoldPerStage = 10; // 1스테이지당 10골드
    private float baseGemChance = 0.02f;  // 1분당 2% 확률로 1잼 지급

    private DateTime rewordStartTime;
    private float elapsedHours; // 경과되는 시간
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
        string formattedTime = string.Format("{0:F1}시간", elapsedHours);
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
        elapsedHours = (float)(passed).TotalHours;

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
        float cappedElapsedHours = Mathf.Min(elapsedHours, 24f); // 최대 24시간 제한

        rewordGold =(Managers.Player.CurrentStage + 1) * baseGoldPerStage * (int)cappedElapsedHours;
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
