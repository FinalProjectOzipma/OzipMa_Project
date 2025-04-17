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

    enum UIObject
    {
        AttackUpgrade,
        DefenceUpgrade,
        RandomUpgrade
    }


    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));


        GetTextMeshProUGUI((int)Texts.GoldText).text = Managers.Economy.GetGold().ToString();
        GetTextMeshProUGUI((int)Texts.ZamText).text = Managers.Economy.GetZam().ToString();
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBack);

    }

    private TextMeshProUGUI GetTextMeshProUGUI(int idx) { return Get<TextMeshProUGUI>(idx); }

    private void OnEnable()
    {
        if (Managers.Economy != null)
        {
            Managers.Economy.OnGoldChanged += UpdateGoldUI;
            UpdateGoldUI(Managers.Economy.GetGold());
        }

 
    }

    private void OnDisable()
    {
        if (Managers.Economy != null)
        {
            Managers.Economy.OnGoldChanged -= UpdateGoldUI;
        }

    }

    private void UpdateGoldUI(long gold)
    {
        GetTextMeshProUGUI((int)Texts.GoldText).text = EconomyManager.FormatNumber(Managers.Economy.GetGold());
        GetTextMeshProUGUI((int)Texts.ZamText).text = EconomyManager.FormatNumber(Managers.Economy.GetZam());
    }

    public void OnClickBack(PointerEventData data)
    {
        this.gameObject.SetActive(false);
        Managers.Audio.AudioControler.PlaySFX(SFXClipName.ButtonClick, this.transform.position);
    }


}
