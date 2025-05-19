using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InfoPopup : UI_Popup
{
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI DescriptionText;
    [SerializeField] private TextMeshProUGUI RankText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI AttackText;
    [SerializeField] private TextMeshProUGUI AttackTypeText;
    [SerializeField] private TextMeshProUGUI AbilityTypeText;
    [SerializeField] private TextMeshProUGUI AttackCoolDownText;
    [SerializeField] private TextMeshProUGUI AttackRangeText;
    [SerializeField] private TextMeshProUGUI MoveSpeedText;
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] private TextMeshProUGUI DefenceText;
    [SerializeField] private Image InfoImage;


    [SerializeField] private Button CloseButton;
    [SerializeField] private Button BGClose;

    [SerializeField] private GameObject UIInfo;
    [SerializeField] private GameObject BGObject;

    [SerializeField] private GameObject NormalWIndow;
    [SerializeField] private GameObject RareWindow;
    [SerializeField] private GameObject EpicWindow;
    [SerializeField] private GameObject LegendWindow;
    [SerializeField] private GameObject MythWindow;
    [SerializeField] private GameObject Description;

    [SerializeField] private Image Speed;
    [SerializeField] private Image Health;
    [SerializeField] private Image Defence;


    public Image[] starArrays = new Image[10];

    private void Awake()
    {
        CloseButton.gameObject.BindEvent(OnClikcBack);
        BGClose.gameObject.BindEvent(OnClikcBack);
    }

    private void Start()
    {
        AnimePopup(UIInfo);
    }

    private void OnEnable()
    {
        if (uiSeq != null) AnimePopup(UIInfo);
    }

    /// <summary>
    /// 정보창 닫기
    /// </summary>
    public void OnClikcBack(PointerEventData data)
    {
        AnimePopup(UIInfo, true);
        Description.SetActive(false);
        uiSeq.Play().OnComplete(() =>
        {
            ClosePopupUI();
        });
    }

    /// <summary>
    /// UI화면에 해당 슬롯에 대한 정보 전달
    /// </summary>
    public void SelectedInfo<T>(T selectedInfo) where T : UserObject, IGettable
    {
        NameText.text = selectedInfo.Name;
        DescriptionText.text = selectedInfo.Description;
        RankText.text = selectedInfo.RankType.ToString();
        LevelText.text = selectedInfo.Status.Level.GetValueToString();
        AttackText.text = selectedInfo.Status.Attack.GetValueToString();
        AttackCoolDownText.text = selectedInfo.Status.AttackCoolDown.GetValueToString();
        AttackRangeText.text = selectedInfo.Status.AttackRange.GetValueToString();

        SelectRankWindow(selectedInfo.RankType);

        SetGradeStarImage(selectedInfo.Status.Grade.GetValue());


        InfoImage.sprite = selectedInfo.Sprite;

        if (selectedInfo is MyUnit myUnit)
        {
            ATKTypes(myUnit);
            AbilliyTypes(myUnit);

            Speed.enabled = true;
            Health.enabled = true;
            Defence.enabled = true;


            MoveSpeedText.text = myUnit.Status.MoveSpeed.GetValueToString();
            HealthText.text = myUnit.Status.Health.GetValueToString("F0");
            DefenceText.text = myUnit.Status.Defence.GetValueToString();

        }
        else if (selectedInfo is Tower tower)
        {
            ATKTypes(tower);
            AbilliyTypes(tower);

            Speed.enabled = false;
            Health.enabled = false;
            Defence.enabled = false;

            MoveSpeedText.text = "";
            HealthText.text = "";
            DefenceText.text = "";
        }

    }


    /// <summary>
    /// 유닛과 타워에 공격타입 선별해서 UI에 반영
    /// </summary>
    private void ATKTypes<T>(T go) where T : UserObject, IGettable
    {
        AtkType? atkType = go switch
        {
            MyUnit myUnit => myUnit.AtkType,
            Tower tower => tower.AtkType,
            _ => null
        };

        if (atkType.HasValue)
        {
            AttackTypeText.text = GetAtkTypeText(atkType.Value);
        }
    }
    private string GetAtkTypeText(AtkType atkType)
    {
        return atkType switch
        {
            AtkType.DirectHit => "직격",
            AtkType.Area => "범위",
            AtkType.Projectile => "투사체",
            AtkType.ReflectDamage => "반사",
            AtkType.VampiricAttack => "흡혈",
            AtkType.StatusInfliction => "혼란",
            _ => "알 수 없음"
        };
    }


    /// <summary>
    /// 유닛과 타워에 특성타입 선별해서 UI에 반영
    /// </summary>
    private void AbilliyTypes<T>(T go) where T : UserObject, IGettable
    {
        AbilityType? abilityType = go switch
        {
            MyUnit myUnit => myUnit.AbilityType,
            Tower tower => tower.TowerType,
            _ => null
        };

        if (abilityType.HasValue)
        {
            AbilityTypeText.text = GetAbillityTypeText(abilityType.Value);
        }
    }

    private string GetAbillityTypeText(AbilityType atkType)
    {
        return atkType switch
        {
            AbilityType.Physical => "물리",
            AbilityType.Psychic => "정신",
            AbilityType.Magic => "마법",
            AbilityType.Fire => "불",
            AbilityType.Explosive => "폭발",
            AbilityType.Dark => "어둠",
            AbilityType.None => "무",
            _ => "알 수 없음"
        };
    }


    /// <summary>
    /// 유닛과 타워 랭크별 배경 변경
    /// </summary>
    public void SelectRankWindow(RankType rankType)
    {
        NormalWIndow.SetActive(false);
        RareWindow.SetActive(false);
        EpicWindow.SetActive(false);
        LegendWindow.SetActive(false);
        MythWindow.SetActive(false);

        switch (rankType)
        {
            case RankType.Normal:
                NormalWIndow.SetActive(true);
                break;
            case RankType.Rare:
                RareWindow.SetActive(true);
                break;
            case RankType.Epic:
                EpicWindow.SetActive(true);
                break;
            case RankType.Legend:
                LegendWindow.SetActive(true);
                break;
            case RankType.Myth:
                MythWindow.SetActive(true);
                break;
        }

    }

    public void SetGradeStarImage(int grade)
    {
        Util.Log("등급 : " + grade.ToString());
        foreach (var image in starArrays)
        {
            image.color = Color.gray;
        }

        for (int i = 0; i <grade; i++ )
        {
            starArrays[i].color = Color.white;
        }
    }

}
