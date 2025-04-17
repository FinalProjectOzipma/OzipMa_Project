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


        GetTextMeshProUGUI((int)Texts.GoldText).text = Managers.Player.GetGold().ToString();
        GetTextMeshProUGUI((int)Texts.ZamText).text = Managers.Player.GetZam().ToString();
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBack);

    }

    private TextMeshProUGUI GetTextMeshProUGUI(int idx) { return Get<TextMeshProUGUI>(idx); }

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
        GetTextMeshProUGUI((int)Texts.GoldText).text = Util.FormatNumber(Managers.Player.GetGold());
        GetTextMeshProUGUI((int)Texts.ZamText).text = Util.FormatNumber(Managers.Player.GetZam());
    }

    public void OnClickBack(PointerEventData data)
    {
        this.gameObject.SetActive(false);
        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick, this.transform.position);
    }


}
