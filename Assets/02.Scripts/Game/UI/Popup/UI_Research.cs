using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Firebase.Database;
using System.Threading.Tasks;
using Unity.Mathematics;

public class UI_Research : UI_Base
{
    [SerializeField] private Button UpgradeButton;
    [SerializeField] private Button GoldSpendButton;
    [SerializeField] private Button ZamSpendButton;
    [SerializeField] private Button CheckButton;

    [SerializeField] private TextMeshProUGUI FillText;
    [SerializeField] private TextMeshProUGUI UpgradeText;
    [SerializeField] private TextMeshProUGUI UpgradeButtonText;
    [SerializeField] private TextMeshProUGUI GoldSpendText;
    [SerializeField] private TextMeshProUGUI ZamSpendText;
    [SerializeField] private TextMeshProUGUI UpdateLevel;

    [SerializeField] private Image Icon;
    [SerializeField] private Image FillBackImage;
    [SerializeField] private Image FillImage;
    [SerializeField] private Image GoldImage;
    [SerializeField] private Image ZamImage;


    enum Objects
    {
        GoldAlarmPopup,
        ZamAlarmPopup,
        StartFailPopup
    }

    [SerializeField] private ParticleSystem StarEffect;

    private float researchDuration; // 연구 시간
    private double elapsedSeconds; // 경과되는 시간
    private bool isResearching = false; // 연구 중인지 아닌지에 대한 불값
    private int updateLevel; // 업데이트 레벨
    private float updateStat;


    private DateTime startTime; // 업그레이드 시작 시간
    private float secondsToReduce = 3600.0f; // 1시간 감소 
    private long spendGold; // 업그레이드 필요 골드
    private long spendZam; // 업그레드 필요 잼

    public ResearchUpgradeType researchUpgradeType; // 연구 타입



    private float baseTime = 300.0f;
    //private float growthFactor = 2.0f;
    private bool isComplete = false;

    private List<MyUnit> myUnitList;
    private List<Tower> towerList;

    private ResearchData researchData;



    private void Awake()
    {
        Init();
        myUnitList = new();
        towerList = new();
    }

    private void Start()
    {

        if (!string.IsNullOrEmpty(researchData.StartTime))
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
        if (isResearching)
        {
            DateTime currentTIme = Managers.Game.ServerUtcNow;

            TimeSpan passed = currentTIme - startTime; // 업그레이드 시작 시간을 기준으로 현재 시간 사이의 차이
            elapsedSeconds = passed.TotalSeconds;
            float progress = Mathf.Clamp01((float)elapsedSeconds / researchDuration);

            FillImage.fillAmount = progress;


            if (progress >= 0.99f)
            {
                FillText.text = "99%/100%";
            }
            else
            {
                FillText.text = Mathf.RoundToInt(progress * 100.0f) + "%" + "/100%";
            }

            float remainingSeconds = Mathf.Max(researchDuration - (float)elapsedSeconds, 0f);
            TimeSpan remainingTime = TimeSpan.FromSeconds(remainingSeconds);
            string formattedTime = string.Format("{0:D1}h {1:D2}m {2:D2}s",
                remainingTime.Hours + remainingTime.Days * 24,
                remainingTime.Minutes,
                remainingTime.Seconds);

            UpgradeText.text = $"남은 시간 : {formattedTime}";

            if (progress >= 1f)
            {
                FillText.text = "100%/100%";
                CompleteResearch();
            }
        }

        if (isResearching)
        {
            ChangeGlodZamButton();
        }
    }


    private void ChangeGlodZamButton()
    {

        if (Managers.Player.Gold < spendGold)
        {
            GoldSpendButton.interactable = false;
            GoldSpendText.color = Color.red;
        }
        else
        {
            GoldSpendButton.interactable = true;
            GoldSpendText.color = Color.white;
        }

        if (Managers.Player.Gem < spendZam)
        {
            ZamSpendButton.interactable = false;
            ZamSpendText.color = Color.red;
        }
        else
        {
            ZamSpendButton.interactable = true;
            ZamSpendText.color = Color.white;
        }

    }

    public override void Init()
    {
        ResearchType(researchUpgradeType);

        updateLevel = researchData.UpdateLevel == 0 ? 1 : researchData.UpdateLevel;
        researchDuration = researchData.ResearchDuration == 0 ? baseTime : researchData.ResearchDuration;


        if (researchData.UpdateStat == 0)
        {
            if (researchUpgradeType != ResearchUpgradeType.Random)
            {
                updateStat = 30.0f;
            }
            else
            {
                updateStat = 40.0f;
            }
        }
        else
        {
            updateStat = researchData.UpdateStat;
        }


        if (researchData.SpendGold == 0)
        {
            if (researchUpgradeType != ResearchUpgradeType.Random)
            {
                spendGold = 800L;
            }
            else
            {
                spendGold = 500L;
            }
        }
        else
        {
            spendGold = researchData.SpendGold;
        }

        if (researchData.SpendZam == 0)
        {
            if (researchUpgradeType != ResearchUpgradeType.Random)
            {
                spendZam = 800L;
            }
            else
            {
                spendZam = 500L;
            }
        }
        else
        {
            spendZam = researchData.SpendZam;
        }

        UpgradeButton.gameObject.BindEvent(OnClickStartResearch); // 업그레드 시작 버튼
        GoldSpendButton.gameObject.BindEvent(OnClickSaveTime); // 골드 사용 시 시간 감소
        ZamSpendButton.gameObject.BindEvent(OnClickCompleteResearch); // 잼 사용 시 연구 완료
        CheckButton.gameObject.BindEvent(OnClickCompleteButton);


        UpdateLevel.text = $"Lv {updateLevel}";
        GoldSpendText.text = Util.FormatNumber(spendGold);
        ZamSpendText.text = Util.FormatNumber(spendZam);

        if (researchUpgradeType != ResearchUpgradeType.Random)
            UpgradeText.text = $"업그레이드 : +{updateStat}";
        else
            UpgradeText.text = $"업그레이드 : +{updateStat - 20.0f}~{updateStat}";

    }

    /// <summary>
    /// 업그레이드 버튼 누르면 호출해서 업그레이드 시작
    /// </summary>
    public void OnClickStartResearch(PointerEventData data)
    {
        if (isResearching) return; // 이미 진행 중이면 무시
        StartTimeCheck();

    }

    // 서비시간과 로컬시간 확인하고 연구 시작
    private async void StartTimeCheck()
    {
        await Managers.Game.Init();
        StartResearch();
    }

    // 연구 시작
    public void StartResearch()
    {
        isComplete = false;


        if (!string.IsNullOrEmpty(researchData.StartTime))
        {
            LoadAndCheckProgress();
            return;
        }

        startTime = Managers.Game.ServerUtcNow;
        researchData.StartTime = startTime.ToString("o");
        researchData.ResearchDuration = researchDuration;

        isResearching = true;
        CheckButton.gameObject.SetActive(false);
        UpgradeButtonText.text = "연구";
        UpgradeButton.interactable = false;
        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);
    }


    /// <summary>
    /// 서버에 저장된 시간과 진행 시간 가져와서 업데이트 해주는 함수
    /// </summary>
    private void LoadAndCheckProgress()
    {
        startTime = DateTime.Parse(researchData.StartTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        researchDuration = researchData.ResearchDuration;

        TimeSpan passed = Managers.Game.ServerUtcNow - startTime;
        float elapsedSeconds = (float)passed.TotalSeconds;

        if (elapsedSeconds >= researchDuration)
        {
            CompleteResearch();
        }
        else
        {
            isResearching = true;
            CheckButton.gameObject.SetActive(false);
            UpgradeButtonText.text = "연구";
            UpgradeButton.gameObject.SetActive(true);
            UpgradeButton.interactable = false;
        }
    }

    /// <summary>
    /// 잼 사용으로 바로 연구 완료
    /// </summary>
    public void OnClickCompleteResearch(PointerEventData data)
    {
        bool isPopup = false;

        if (isPopup) return;

        isPopup = true;

        if (!isResearching)
        {
            isPopup = false;
            return;
        }

        if (Managers.Player.Gem < spendZam)
        {
            Managers.UI.ShowPopupUI<UI_Alarm>(Objects.ZamAlarmPopup.ToString());
            //Managers.UI.GetAlarmPopup().WriteText("잼이 부족합니다. 잼이 없네요.");
            isPopup = false;
            return;
        }

        Managers.Player.SpenZam(spendZam);
        elapsedSeconds = researchDuration;

        CompleteResearch();
        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);

        if (isPopup)
        {
            isPopup = false;
        }

    }


    /// <summary>
    /// 골드 소모하여 시간 단축
    /// </summary>
    public void OnClickSaveTime(PointerEventData data)
    {
        bool isPopup = false;

        if (isPopup) return;

        isPopup = true;

        if (!isResearching)
        {
            isPopup = false;
            return;
        }

        if (Managers.Player.Gold < spendGold)
        {
            Managers.UI.ShowPopupUI<UI_Alarm>(Objects.GoldAlarmPopup.ToString());
            isPopup = false;
            return;
        }
        Managers.Player.SpenGold(spendGold);
        startTime = startTime.AddSeconds(-secondsToReduce);

        researchData.StartTime = startTime.ToString("o");
        researchData.ResearchDuration = researchDuration;
        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);

        if (isPopup)
        {
            isPopup = false;
        }
    }


    /// <summary>
    /// 업그레이드 완료 함수
    /// </summary>
    void CompleteResearch()
    {
        isResearching = false;
        UpgradeButton.gameObject.SetActive(false);
        CheckButton.gameObject.SetActive(true);
        FillImage.fillAmount = 1.0f;
        FillText.text = "100%/100%";
        UpgradeText.text = $"업그레이드 완료";
        UpgradeButtonText.text = "시작";
        GoldSpendButton.interactable = false;
        ZamSpendButton.interactable = false;
        GoldSpendText.color = Color.white;
        ZamSpendText.color = Color.white;
    }



    /// <summary>
    /// 저장된 값 없으면 초기값 0으로 UI 표시
    /// </summary>
    void ResetProgress()
    {
        FillImage.fillAmount = 0.0f;
        FillText.text = "0%/100%";
        GoldSpendButton.interactable = false;
        ZamSpendButton.interactable = false;
    }

    // 완료 버튼 클릭 매서드
    private void OnClickCompleteButton(PointerEventData data)
    {
        if (isComplete) return;
        isComplete = true;

        _ = HandleCheckButton();
    }

    // 치트 검사
    private async Task HandleCheckButton()
    {
        bool IsCheat = await CheckTimeCheat();

        if (IsCheat)
        {
            StartTimeCheck();

        }
        else
        {
            OnUpgrade();
        }
    }

    // 서버시간 가져와서 저장된 시작값 차이 계산
    private async Task<bool> CheckTimeCheat()
    {
        await Managers.Game.Init();

        DateTime storedStartTime = DateTime.Parse(researchData.StartTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        DateTime now = Managers.Game.ServerUtcNow;
        TimeSpan elapsed = now - storedStartTime;

        return (Math.Abs(Managers.Game.ServerTimeOffset) > 30);

    }

    // 치트가 아니면 업그레이드 시작
    private void OnUpgrade()
    {

        StarEffect.Play();
        Managers.Audio.audioControler.PlaySFX(SFXClipName.Upgrade);

        UpgradeButton.gameObject.SetActive(true);
        CheckButton.gameObject.SetActive(false);
        UpgradeButton.interactable = true;


        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);


        FillImage.fillAmount = 0.0f;
        FillText.text = "0%/100%";


        updateLevel++;

        if (researchDuration < 3600.0f)
        {
            switch (updateLevel)
            {
                case 2:
                    researchDuration = 600.0f;
                    break;
                case 3:
                    researchDuration = 1800.0f;
                    break;
                case 4:
                    researchDuration = 3600.0f;
                    break;
            }
        }
        else if (researchDuration < 43200.0f)
        {
            researchDuration += 1800.0f;
        }
        else
        {
            researchDuration = 43200.0f;
        }

        StatUpgrade(researchUpgradeType); // 스탯 업그레이드

        updateStat += researchUpgradeType != ResearchUpgradeType.Random ? 10.0f : 20.0f;
        spendGold += researchUpgradeType != ResearchUpgradeType.Random ? 1000L : 500L;
        spendZam += researchUpgradeType != ResearchUpgradeType.Random ? 1000L : 500L;

        researchData.StartTime = "";
        researchData.ResearchDuration = researchDuration;
        researchData.UpdateLevel = updateLevel;
        researchData.UpdateStat = updateStat;
        researchData.SpendGold = spendGold;
        researchData.SpendZam = spendZam;

        UpdateLevel.text = $"Lv {updateLevel}";

        if (researchUpgradeType != ResearchUpgradeType.Random)
            UpgradeText.text = $"업그레이드 : +{updateStat}";
        else
            UpgradeText.text = $"업그레이드 : +{updateStat - 20.0f}~{updateStat}";


        GoldSpendText.text = Util.FormatNumber(spendGold);
        ZamSpendText.text = Util.FormatNumber(spendZam);

    }

    // 실제 유닛과 타워 스탯 업그레이드
    public void StatUpgrade(ResearchUpgradeType upgradeType)
    {
        SeperatedIGettable<MyUnit>(myUnitList);
        SeperatedIGettable<Tower>(towerList);

        switch (upgradeType)
        {
            case ResearchUpgradeType.Attack:

                foreach (var unitAttack in myUnitList)
                {
                    unitAttack.Status.Attack.AddValue(updateStat);
                    Util.Log("유닛공격력:" + unitAttack.Status.Attack.GetValue().ToString());
                }

                foreach (var towerAttack in towerList)
                {
                    towerAttack.TowerStatus.Attack.AddValue(updateStat);
                }
                break;
            case ResearchUpgradeType.Defence:

                foreach (var unitDefence in myUnitList)
                {
                    MyUnitStatus defenceStatus = unitDefence.Status as MyUnitStatus;

                    defenceStatus.Defence.AddValue(updateStat);
                }

                break;
            case ResearchUpgradeType.Random:

                float randomStat = UnityEngine.Random.Range(updateStat - 20.0f, updateStat);
                int randomStatus = UnityEngine.Random.Range(1, 101);

                if (randomStatus < 50)
                {
                    foreach (var unitAttack in myUnitList)
                    {
                        unitAttack.Status.Attack.AddValue(randomStat);
                    }

                    foreach (var towerAttack in towerList)
                    {
                        towerAttack.TowerStatus.Attack.AddValue(randomStat);
                    }

                }
                else
                {
                    foreach (var unitDefence in myUnitList)
                    {
                        MyUnitStatus defenceStatus = unitDefence.Status as MyUnitStatus;

                        defenceStatus.Defence.AddValue(updateStat);
                    }
                }
                break;
            case ResearchUpgradeType.Core:
                CoreController core = Managers.Wave.MainCore.GetComponent<CoreController>();
                core.core.Health.MaxValue += updateStat;
                core.core.Health.SetValue(core.core.Health.MaxValue);
                core.core.CoreLevel.SetValue(updateLevel);
                core.CoreUpgrade();

                Managers.Player.MainCoreData.Health.MaxValue += updateStat;
                Managers.Player.MainCoreData.Health.SetValue(Managers.Player.MainCoreData.Health.MaxValue);
                Managers.Player.MainCoreData.CoreLevel.SetValue(updateLevel);
                break;
        }

    }

    // 유닛과 타워 인벤에서 분리해서 리스트 생성
    private void SeperatedIGettable<T>(List<T> list) where T : IGettable
    {
        list.Clear();

        var rawList = Managers.Player.Inventory.GetList<T>();
        if (rawList == null) return;

        foreach (var item in rawList)
        {
            if (item is T unit)
            {
                list.Add(unit);
            }
        }
    }


    private void ResearchType(ResearchUpgradeType researchUpgradeType)
    {
        switch (researchUpgradeType)
        {
            case ResearchUpgradeType.Attack:
                researchData = Managers.Player.AttackResearchData;
                break;
            case ResearchUpgradeType.Defence:
                researchData = Managers.Player.DefenceResearchData;
                break;
            case ResearchUpgradeType.Core:
                researchData = Managers.Player.CoreResearchData;
                break;
            case ResearchUpgradeType.Random:
                researchData = Managers.Player.RandomResearchData;
                break;
        }
    }

}
