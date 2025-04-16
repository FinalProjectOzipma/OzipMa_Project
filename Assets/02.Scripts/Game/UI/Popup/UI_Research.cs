using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Research : UI_Base
{
    enum Buttons
    {
        UpgradeButton,
        GoldSpendButton,
        JamSpendButton

    }

    enum Texts
    {
        FillText,
        UpgradeText,
        UpgradeButtonText,
        GoldSpendText,
        JamSpendText,
        UpdateLevel

    }


    enum Images
    {
        Icon,
        FillBackImage,
        FillImage

    }

    public enum ResearchUpgradeType
    {
        Attack,
        Defence,
        Random
    }



    private float researchDuration; // 연구 시간
    private float elapsedSeconds; // 경과되는 시간
    private bool isResearching = false; // 연구 중인지 아닌지에 대한 불값
    private int updateLevel; // 업데이트 레벨

    private DateTime startTime; // 업그레이드 시작 시간
    private float secondsToReduce = 10.0f;

    public ResearchUpgradeType researchUpgradeType;

    private string startKey;
    private string durationKey;
    private string levelKey;


    private void Start()
    {
        Init();

        if (PlayerPrefs.HasKey(startKey))
        {
            // 재접속 시 시간 계산
            LoadAndCheckProgress();
        }
        else
        {
            ResetProgress();
        }
    }

    private void Update()
    {
        if(isResearching)
        {
            TimeSpan passed = DateTime.UtcNow - startTime; // 업그레이드 시작 시간을 기준으로 현재 시간 사이의 차이
            elapsedSeconds = (float)passed.TotalSeconds;
            float progress = Mathf.Clamp01(elapsedSeconds / researchDuration);

            GetImage((int)Images.FillImage).fillAmount = progress;
            GetTextMeshProUGUI((int)Texts.FillText).text = Mathf.RoundToInt(progress * 100f) + "%" + "/100%";

            float remainingSeconds = Mathf.Max(researchDuration - elapsedSeconds, 0f);
            TimeSpan remainingTime = TimeSpan.FromSeconds(remainingSeconds);
            string formattedTime = string.Format("{0:D1}h {1:D2}m {2:D2}s",
                remainingTime.Hours + remainingTime.Days * 24,
                remainingTime.Minutes,
                remainingTime.Seconds);

            GetTextMeshProUGUI((int)Texts.UpgradeText).text = $"남은 시간 : {formattedTime}";

            if (progress >= 1f)
            {
                CompleteResearch();
            }
        }     
    }

    public override void Init()
    {

        startKey = $"ResearchStartTime_{researchUpgradeType}";
        durationKey = $"ResearchDuration_{researchUpgradeType}";
        levelKey = $"ResearchLevel_{researchUpgradeType}";

        if (!PlayerPrefs.HasKey(levelKey))
        {
            updateLevel = 1;
        }
        else
        {
            updateLevel = PlayerPrefs.GetInt(levelKey);
        }

        if (!PlayerPrefs.HasKey(durationKey))
        {
            researchDuration = 10.0f;
        }
        else
        {
            researchDuration = PlayerPrefs.GetFloat(durationKey);
        }
            
        
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.UpgradeButton).gameObject.BindEvent(StartResearch); // 업그레드 시작 버튼
        GetButton((int)Buttons.GoldSpendButton).gameObject.BindEvent(OnClickSaveTime); // 골드 사용 시 시간 감소
        GetButton((int)Buttons.JamSpendButton).gameObject.BindEvent(OnClickCompleteResearch); // 잼 사용 시 연구 완료
        GetTextMeshProUGUI((int)Texts.UpdateLevel).text = $"Lv {updateLevel}";

    }

    private TextMeshProUGUI GetTextMeshProUGUI(int idx) { return Get<TextMeshProUGUI>(idx); }


    /// <summary>
    /// 업그레이드 버튼 누르면 호출해서 업그레이드 시작
    /// </summary>
    public void StartResearch(PointerEventData data)
    {
        if (isResearching) return; // 이미 진행 중이면 무시

        startTime = DateTime.UtcNow;
        PlayerPrefs.SetString(startKey, startTime.ToString());
        PlayerPrefs.SetFloat(durationKey, researchDuration);
        PlayerPrefs.Save();

        isResearching = true;
        GetButton((int)Buttons.UpgradeButton).interactable = false;
    }


    /// <summary>
    /// 잼 사용으로 바로 연구 완료
    /// </summary>
    public void OnClickCompleteResearch(PointerEventData data)
    {
        if (!isResearching) return;

        elapsedSeconds = researchDuration;

        CompleteResearch();
    }


    /// <summary>
    /// 골드 소모하여 시간 단축
    /// </summary>
    public void OnClickSaveTime(PointerEventData data)
    {
        if (!isResearching) return;
        startTime = startTime.AddSeconds(-secondsToReduce);
    }


    /// <summary>
    /// 업그레이드 완료 함수
    /// </summary>
    void CompleteResearch()
    {
        isResearching = false;
        GetImage((int)Images.FillImage).fillAmount = 0.0f;
        GetTextMeshProUGUI((int)Texts.FillText).text = "0%/100%";
        GetButton((int)Buttons.UpgradeButton).interactable = true;
        researchDuration += 10.0f;
        updateLevel++;
        PlayerPrefs.DeleteKey(startKey);
        PlayerPrefs.SetFloat(durationKey, researchDuration);
        PlayerPrefs.SetInt(levelKey, updateLevel);
        PlayerPrefs.Save();
        StatUpgrade(researchUpgradeType); // 스탯 업그레이드
       
        GetTextMeshProUGUI((int)Texts.UpdateLevel).text = $"Lv {updateLevel}";
        GetTextMeshProUGUI((int)Texts.UpgradeText).text = $"업그레이드";
        Debug.Log($"다음 연구시간 : {researchDuration}");
    }

    /// <summary>
    /// PlayerPrefs에 저장된 시간과 진행 시간 가져와서 업데이트 해주는 함수
    /// </summary>
    private void LoadAndCheckProgress()
    {
        startTime = DateTime.Parse(PlayerPrefs.GetString(startKey));
        researchDuration = PlayerPrefs.GetFloat(durationKey);

        TimeSpan passed = DateTime.UtcNow - startTime;
        float elapsedSeconds = (float)passed.TotalSeconds;

        if (elapsedSeconds >= researchDuration)
        {
            CompleteResearch();
        }
        else
        {
            isResearching = true;
        }
    }

    /// <summary>
    /// PlayerPrefs에 없으면 초기값 0으로 UI 표시
    /// </summary>
    void ResetProgress()
    {
        GetImage((int)Images.FillImage).fillAmount = 0.0f;
        GetTextMeshProUGUI((int)Texts.FillText).text ="0%/100%";
    }

    // 이거 데이터를 어디서 받아와야 될까?
    // 1) 인벤토리 2) Pool 3) DataManager
    public void StatUpgrade(ResearchUpgradeType upgradeType)
    {
        switch(upgradeType)
        {
            case ResearchUpgradeType.Attack:
                break;
            case ResearchUpgradeType.Defence:
                break;
            case ResearchUpgradeType.Random:
                break;
        }

    }


}
