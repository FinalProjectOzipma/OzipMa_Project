using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_MainTest : UI_Scene
{
    enum Texts
    {
        MainGoldText,
        MainZamText
    }



    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));

        Get<TextMeshProUGUI>((int)Texts.MainGoldText).text = Util.FormatNumber(Managers.Player.GetGold());
        Get<TextMeshProUGUI>((int)Texts.MainZamText).text = Util.FormatNumber(Managers.Player.GetZam());
    }

    private void OnEnable()
    {
        if (Managers.Player != null)
        {
            Managers.Player.OnGoldChanged += UpdateGoldUI;
            UpdateGoldUI(Managers.Player.GetGold());
        }
    }

    private void OnDisable()
    {
        if (Managers.Player != null)
        {
            Managers.Player.OnGoldChanged -= UpdateGoldUI;
        }

    }

    private void UpdateGoldUI(long gold)
    {
        Get<TextMeshProUGUI>((int)Texts.MainGoldText).text = Util.FormatNumber(Managers.Player.GetGold());
        Get<TextMeshProUGUI>((int)Texts.MainZamText).text = Util.FormatNumber(Managers.Player.GetZam());
    }

}
