using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackAndAbilityTypeSlot : UI_Base
{
    public enum SlotType
    {
        AtkType,
        AbilityType
    }

    [SerializeField] private Button ToolTipButton;

    public TextMeshProUGUI description;
    public TextMeshProUGUI typeName;
    public SlotType slotType;
    List<DefaultTable.AttackDefault> attacks;
    List<DefaultTable.AbilityDefaultValue> abilities;

    private void Awake()
    {
        attacks = Util.TableConverter<DefaultTable.AttackDefault>(Managers.Data.Datas[Enums.Sheet.AttackDefault]);
        abilities = Util.TableConverter<DefaultTable.AbilityDefaultValue>(Managers.Data.Datas[Enums.Sheet.AbilityDefaultValue]);
    }

    private void Start()
    {
        ToolTipButton.onClick.AddListener(OnCilckDescription);
    }

    /// <summary>
    /// 클릭 시 해당하는 특성 UI에 표시
    /// </summary>
    public void OnCilckDescription()
    {

        switch (slotType)
        {
            case SlotType.AtkType:
                AtkType atk = GetAtkTypeText(typeName.text);
                for (int i = 0; i < attacks.Count; i++)
                {
                    if (atk == attacks[i].AttackType)
                    {
                        description.gameObject.SetActive(true);
                        Util.Log("버튼 클릭 됨");
                        description.text = attacks[i].Description;
                        break;
                    }
                }
                break;
            case SlotType.AbilityType:
                AbilityType ability = GetAbillityTypeText(typeName.text);
                for (int i = 0; i < abilities.Count; i++)
                {
                    if (ability == abilities[i].AbilityType)
                    {
                        description.gameObject.SetActive(true);
                        description.text = abilities[i].Description;
                        break;
                    }
                }

                break;
        }

    }

    /// <summary>
    /// text 읽어와서 해당 공격타입 변환
    /// </summary>
    private AtkType GetAtkTypeText(string typeName)
    {
        return typeName switch
        {
            "직격" => AtkType.DirectHit,
            "범위" => AtkType.Area,
            "투사체" => AtkType.Projectile,
            "반사" => AtkType.ReflectDamage,
            "흡혈" => AtkType.VampiricAttack,
            "혼란" => AtkType.StatusInfliction,
            _ => AtkType.Count
        };
    }

    /// <summary>
    /// text 읽어와서 해당 능력타입 반환
    /// </summary>
    private AbilityType GetAbillityTypeText(string typeName)
    {
        return typeName switch
        {
            "물리" => AbilityType.Physical,
            "정신" => AbilityType.Psychic,
            "마법" => AbilityType.Magic,
            "불" => AbilityType.Fire,
            "폭발" => AbilityType.Explosive,
            "어둠" => AbilityType.Dark,
            "무" => AbilityType.None,
            _ => AbilityType.Count
        };
    }

}
