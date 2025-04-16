using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ResearchScene : UI_Base
{
    enum Buttons
    {
        BackButton
    }

    enum Texts
    {
        GoldText,
        ZamText
    }

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        GetTextMeshProUGUI((int)Texts.GoldText).text = GoldBank.instance.GetGold().ToString();
        GetTextMeshProUGUI((int)Texts.ZamText).text = GoldBank.instance.GetZam().ToString();
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBack);

    }

    private TextMeshProUGUI GetTextMeshProUGUI(int idx) { return Get<TextMeshProUGUI>(idx); }

    private void OnEnable()
    {
        if (GoldBank.instance != null)
        {
            GoldBank.instance.OnGoldChanged += UpdateGoldUI;
            UpdateGoldUI(GoldBank.instance.GetGold());
        }
    }

    private void OnDisable()
    {
        if (GoldBank.instance != null)
        {
            GoldBank.instance.OnGoldChanged -= UpdateGoldUI;
        }

    }

    private void UpdateGoldUI(int gold)
    {
        GetTextMeshProUGUI((int)Texts.GoldText).text = GoldBank.instance.GetGold().ToString();
        GetTextMeshProUGUI((int)Texts.ZamText).text = GoldBank.instance.GetZam().ToString();
    }

    public void OnClickBack(PointerEventData data)
    {
        this.gameObject.SetActive(false);
    }


}
