using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_OFFLinePopup : UI_Popup
{
    [SerializeField] private Button CheckButton;
    [SerializeField] private Button DoubleButton;
    [SerializeField] private Button RewardBox;

    [SerializeField] TextMeshProUGUI TimerText;
    [SerializeField] TextMeshProUGUI RewordGoldText;
    [SerializeField] TextMeshProUGUI RewordZamText;

    [SerializeField] private Image CheckImage;
    [SerializeField] private Image DoubleImage;

    [SerializeField] private GameObject UIOffLine;

    [SerializeField] private GameObject BGObject;
    [SerializeField] private GameObject OnBronze;
    [SerializeField] private GameObject OffBronze;
    [SerializeField] private GameObject OnSilver;
    [SerializeField] private GameObject OffSilver;
    [SerializeField] private GameObject OnGold;
    [SerializeField] private GameObject OffGold;

    bool isClick = false;

    private int baseGoldPerMinute = 1; // 1분당 1골드
    private float baseGemChance = 0.02f;  // 1분당 2% 확률로 1잼 지급

    private DateTime rewordStartTime;
    private float elapsedMinute; // 경과되는 시간
    private int rewordGold;
    private int rewordGem;

    private bool isRewordClaimed = false;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(Managers.Player.RewordStartTime))
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


        if (hours != 0)
        {
            string formattedTime = string.Format("{0}시간 {1}분", hours, minutes);
            TimerText.text = formattedTime;
        }
        else
        {
            string formattedTime = string.Format("{0}분", minutes);
            TimerText.text = formattedTime;
        }

        RewordGoldText.text = rewordGold.ToString();
        RewordZamText.text = rewordGem.ToString();


    }

    public override void Init()
    {
        uiSeq = Util.RecyclableSequence();

        CheckButton.gameObject.BindEvent(OnClickReword);
        DoubleButton.gameObject.BindEvent(OnClickDoubleReword);
        RewardBox.gameObject.BindEvent(SwitchToOpenBox);


        rewordStartTime = DateTime.Parse(Managers.Player.RewordStartTime, null, System.Globalization.DateTimeStyles.RoundtripKind);

        TimeSpan passed = Managers.Game.LastSyncedServerTime - rewordStartTime; // 오프라인 경과 시간
        elapsedMinute = (float)(passed).TotalMinutes;

        OffRewardImage();

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
        else if (cappedElapsedHours < 240.0f)
        {
            OffBronze.SetActive(true);
        }
        else if (cappedElapsedHours < 720.0f)
        {
            OffSilver.SetActive(true);
        }
        else
        {
            OffGold.SetActive(true);
        }

        rewordGold = (Managers.Player.CurrentStage + 1) * baseGoldPerMinute * (int)cappedElapsedHours;
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


    private void OffRewardImage()
    {
        BGObject.SetActive(false);
        OnBronze.SetActive(false);
        OffBronze.SetActive(false);
        OnSilver.SetActive(false);
        OffSilver.SetActive(false);
        OnGold.SetActive(false);
        OffGold.SetActive(false);
    }

    public void SwitchToOpenBox(PointerEventData data)
    {
        if (isClick) return;

        isClick = true;

        GameObject currentClosedBox;
        GameObject currentOpenedBox;

        // 1. 어떤 상자가 켜졌는지 찾기
        if (OffBronze.activeSelf)
        {
            currentClosedBox = OffBronze;
            currentOpenedBox = OnBronze;
        }
        else if (OffSilver.activeSelf)
        {
            currentClosedBox = OffSilver;
            currentOpenedBox = OnSilver;
        }
        else
        {
            currentClosedBox = OffGold;
            currentOpenedBox = OnGold;
        }

        // 2. 상자 흔들림 연출
        currentClosedBox.transform
            .DOPunchRotation(new Vector3(0, 0, 10f), 1.0f, 10, 5.0f)
            .OnComplete(() =>
            {
                currentClosedBox.SetActive(false);
                currentOpenedBox.SetActive(true);

                DOVirtual.DelayedCall(0.3f, () =>
                {
                    Managers.Audio.PlaySFX(SFXClipName.Coin);
                    ShowRewardPopup();
                });
            });

    }

    private void ShowRewardPopup()
    {
        BGObject.SetActive(true);

        Sequence popupSequence = DOTween.Sequence();
        popupSequence.Append(BGObject.transform.DOScale(1.1f, 0.25f).SetEase(Ease.OutBack));
        popupSequence.Append(BGObject.transform.DOScale(1.0f, 0.15f).SetEase(Ease.InOutSine));
    }


}
