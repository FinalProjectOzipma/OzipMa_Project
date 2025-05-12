using DefaultTable;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject UIInfo;

    private void Awake()
    {
        CloseButton.gameObject.BindEvent(OnClikcBack);
    }

    private void Start()
    {
        AnimePopup(UIInfo);
    }

    private void OnEnable()
    {
        if(uiSeq != null) AnimePopup(UIInfo);
    }

    public void OnClikcBack(PointerEventData data)
    {
        AnimePopup(UIInfo, true);

        uiSeq.Play().OnComplete(() =>
        {
            ClosePopupUI();
        });
    }

    public void SelectedInfo<T>(T selectedInfo) where T : UserObject, IGettable
    {
        NameText.text = selectedInfo.Name;
        DescriptionText.text = selectedInfo.Description;
        RankText.text = "랭크: " + selectedInfo.RankType.ToString();
        LevelText.text = "Lv. " + selectedInfo.Status.Level.GetValueToString();
        AttackText.text = "공격력: " + selectedInfo.Status.Attack.GetValueToString();
        AttackCoolDownText.text =  "공속: " + selectedInfo.Status.AttackCoolDown.GetValueToString();
        AttackRangeText.text = "사거리: "+ selectedInfo.Status.AttackRange.GetValueToString();

        InfoImage.sprite = selectedInfo.Sprite;

        if (selectedInfo is MyUnit myUnit)
        {
            ATKTypes(myUnit);
            AbilliyTypes(myUnit);

            MoveSpeedText.text = "속도: " + myUnit.Status.MoveSpeed.GetValueToString();
            HealthText.text = "체력: " + myUnit.Status.Health.GetValueToString();
            DefenceText.text = "방어력: " + myUnit.Status.Defence.GetValueToString();

        }
        else if (selectedInfo is Tower tower)
        {
            ATKTypes(tower);
            AbilliyTypes(tower);

            MoveSpeedText.text = "";
            HealthText.text = "";
            DefenceText.text = "";
        }

    }

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

   private void AbilliyTypes<T>(T go) where T : UserObject, IGettable
    {
        AbilityType? abilityType = go switch
        {
            MyUnit myUnit => myUnit.AbilityType,
            Tower tower => tower.TowerType,
            _ => null
        };

        if(abilityType.HasValue)
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

}
