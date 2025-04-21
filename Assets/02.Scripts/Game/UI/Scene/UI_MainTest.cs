using DG.Tweening;
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

        Get<TextMeshProUGUI>((int)Texts.MainGoldText).text = Util.FormatNumber(Managers.Player.GetGold());
        Get<TextMeshProUGUI>((int)Texts.MainZamText).text = Util.FormatNumber(Managers.Player.GetZam());         

    }

    public void OnClickOpenSetting(PointerEventData data)
    {


        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick, this.transform.position);
    }

    public void OnClickOpenResearch(PointerEventData data)
    {
        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick, this.transform.position);
        GetObject((int)UIObject.UI_Research).SetActive(true);
        
    }

    private void PopUpShow(UIObject uIObject)
    {
        GetObject((int)uIObject).SetActive(true);

        var sequence = DOTween.Sequence();

        sequence.Append(GetObject((int)uIObject).transform.DOScale(1.1f, 0.2f));
        sequence.Append(GetObject((int)uIObject).transform.DOScale(1f, 0.1f));

        sequence.Play();

    }

    private void OnClickButtonAnime(UIObject uIObject)
    {
        var sequence = DOTween.Sequence();

        sequence.Append(GetButton((int)Buttons.SettingButton).gameObject.transform.DOScale(0.95f, 0.1f));
        sequence.Append(GetButton((int)Buttons.SettingButton).gameObject.transform.DOScale(1.05f, 0.1f));
        sequence.Append(GetButton((int)Buttons.SettingButton).gameObject.transform.DOScale(1.0f, 0.1f));

        sequence.Play().OnComplete(() =>
        {
            PopUpShow(uIObject);
        });
    }
}
