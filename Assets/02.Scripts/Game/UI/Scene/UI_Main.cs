using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_Main : UI_Scene
{

    enum Buttons
    {
        ResearchButton,
        SettingButton
    }

    enum Texts
    {
        MainGoldText,
        MainZamText,
        StageLv,
        PlayerName
    }

    enum Images
    {
        ProfileImage,
        ResearchButtonImage,
        SettingImage
    }

    enum Objects
    {
        ReseachUI,
        SoundUI
    }

    bool isButton = false;


    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        Get<TextMeshProUGUI>((int)Texts.MainGoldText).text = Util.FormatNumber(Managers.Player.GetGold());
        Get<TextMeshProUGUI>((int)Texts.MainZamText).text = Util.FormatNumber(Managers.Player.GetZam());
        Get<Button>((int)Buttons.ResearchButton).gameObject.BindEvent(OnClikButtonResearch);
        Get<Button>((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSetting);
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

    private void OnClikButtonResearch(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick, this.transform.position);

        var seq = DOTween.Sequence();

        seq.Append(Get<Image>((int)Images.ResearchButtonImage).transform.DOScale(0.9f, 0.1f));
        seq.Append(Get<Image>((int)Images.ResearchButtonImage).transform.DOScale(1.1f, 0.1f));
        seq.Append(Get<Image>((int)Images.ResearchButtonImage).transform.DOScale(1.0f, 0.1f));

        seq.Play().OnComplete(() =>
        {
            Managers.UI.ShowPopupUI<UI_ResearchScene>(Objects.ReseachUI.ToString());
            isButton = false;
        });

    }

    private void OnClickSetting(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick, this.transform.position);


        var seq = DOTween.Sequence();

        seq.Append(Get<Image>((int)Images.SettingImage).transform.DOScale(0.9f, 0.1f));
        seq.Append(Get<Image>((int)Images.SettingImage).transform.DOScale(1.1f, 0.1f));
        seq.Append(Get<Image>((int)Images.SettingImage).transform.DOScale(1.0f, 0.1f));

        seq.Play().OnComplete(() =>
        {
            Managers.UI.ShowPopupUI<UI_Setting>(Objects.SoundUI.ToString());
            isButton = false;
        });
      
    }

 

}
