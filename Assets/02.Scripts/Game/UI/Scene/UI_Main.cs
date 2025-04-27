using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_Main : UI_Scene
{

    enum Buttons
    {
        ResearchButton,
        ManagerButton,
        SettingButton

    }

    enum Texts
    {
        MainGoldText,
        MainZamText,
        StageLv,
        PlayerName,
        ResearchText,
        ManagerText
    }

    enum Images
    {
        ProfileImage,
        ResearchButtonImage,
        ManagerButtonImage,
        SettingImage,
        ProgressImage,
        CompleteImage
    }

    enum Objects
    {
        ReseachUI,
        SoundUI,
        AlarmPopup
    }

    Button ManagerButton;
    Button ResearchButton;
    bool isButton = false;


    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        Managers.UI.SetSceneList<UI_Main>(this);

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        Get<TextMeshProUGUI>((int)Texts.MainGoldText).text = Util.FormatNumber(Managers.Player.GetGold());
        Get<TextMeshProUGUI>((int)Texts.MainZamText).text = Util.FormatNumber(Managers.Player.GetZam());
        Get<Button>((int)Buttons.ResearchButton).gameObject.BindEvent(OnClikButtonResearch);
        Get<Button>((int)Buttons.ManagerButton).gameObject.BindEvent(OnClickManager);
        Get<Button>((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSetting);

        if (Managers.Player != null)
        {
            Managers.Player.OnGoldChanged += UpdateGoldUI;
            UpdateGoldUI(Managers.Player.GetGold());
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

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);

        var seq = DOTween.Sequence();

        //seq.Append(Get<Image>((int)Images.ResearchButtonImage).transform.DOScale(0.9f, 0.1f));
        //seq.Join(Get<TextMeshProUGUI>((int)Texts.ResearchText).transform.DOScale(0.9f, 0.1f));
        //seq.Append(Get<Image>((int)Images.ResearchButtonImage).transform.DOScale(1.1f, 0.1f));
        //seq.Join(Get<TextMeshProUGUI>((int)Texts.ResearchText).transform.DOScale(1.1f, 0.1f));
        //seq.Append(Get<Image>((int)Images.ResearchButtonImage).transform.DOScale(1.0f, 0.1f));
        //seq.Join(Get<TextMeshProUGUI>((int)Texts.ResearchText).transform.DOScale(1.0f, 0.1f));

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

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);


        var seq = DOTween.Sequence();

        //seq.Append(Get<Image>((int)Images.SettingImage).transform.DOScale(0.9f, 0.1f));
        //seq.Append(Get<Image>((int)Images.SettingImage).transform.DOScale(1.1f, 0.1f));
        //seq.Append(Get<Image>((int)Images.SettingImage).transform.DOScale(1.0f, 0.1f));

        seq.Play().OnComplete(() =>
        {
            Managers.UI.ShowPopupUI<UI_Setting>(Objects.SoundUI.ToString());
            isButton = false;
        });
      
    }

    private void OnClickManager(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);
        

        var seq = DOTween.Sequence();

        //seq.Append(Get<Image>((int)Images.ManagerButtonImage).transform.DOScale(0.9f, 0.1f));
        //seq.Join(Get<TextMeshProUGUI>((int)Texts.ManagerText).transform.DOScale(0.9f, 0.1f));
        //seq.Append(Get<Image>((int)Images.ManagerButtonImage).transform.DOScale(1.1f, 0.1f));
        //seq.Join(Get<TextMeshProUGUI>((int)Texts.ManagerText).transform.DOScale(1.1f, 0.1f));
        //seq.Append(Get<Image>((int)Images.ManagerButtonImage).transform.DOScale(1.0f, 0.1f));
        //seq.Join(Get<TextMeshProUGUI>((int)Texts.ManagerText).transform.DOScale(1.0f, 0.1f));

        seq.Play().OnComplete(() =>
        {
            Managers.UI.GetSceneList<InventoryUI>().OnSwipe();
            OffButton();
            isButton = false;
        });

    }

    public void OffButton()
    {
        Get<Button>((int)Buttons.ManagerButton).gameObject.SetActive(false);
        Get<Button>((int)Buttons.ResearchButton).gameObject.SetActive(false);
    }

    public void OnButton()
    {
        Get<Button>((int)Buttons.ManagerButton).gameObject.SetActive(true);
        Get<Button>((int)Buttons.ResearchButton).gameObject.SetActive(true);
    }
}
