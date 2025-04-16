using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MainTest : UI_Base
{
    enum Buttons
    {
        SettingButton,
        ResearchButton
    }

    enum Texts
    {
        MainGoldText,
        MainZamText
    }

    enum UIObject
    {
        UI_Research,
        UI_Sound
    }


    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(UIObject));

        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickOpenSetting);
        GetButton((int)Buttons.ResearchButton).gameObject.BindEvent(OnClickOpenResearch);

        GetObject((int)UIObject.UI_Sound).SetActive(false);
        GetObject((int)UIObject.UI_Research).SetActive(false);

        Get<TextMeshProUGUI>((int)Texts.MainGoldText).text = GoldBank.instance.GetGold().ToString();
        Get<TextMeshProUGUI>((int)Texts.MainZamText).text = GoldBank.instance.GetGold().ToString();
    }

    public void OnClickOpenSetting(PointerEventData data)
    {
        GetObject((int)UIObject.UI_Sound).SetActive(true);
    }

    public void OnClickOpenResearch(PointerEventData data)
    {
        GetObject((int)UIObject.UI_Research).SetActive(true);
    }
}
