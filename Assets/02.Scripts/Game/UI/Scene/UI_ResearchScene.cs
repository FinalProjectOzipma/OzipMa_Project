using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.Numerics;
using UnityEngine;

public class UI_ResearchScene : UI_Scene
{
    enum Buttons
    {
        ResearchButton,
        BackButton
    }

    enum Texts
    {
        GoldText,
        ZamText
    }

    enum Images
    {
        ResearchButtonImage,
        BackImage
    }

    enum ReseachObject
    {
        UI_Research
    }

    Sequence sequence;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(ReseachObject));


        Get<TextMeshProUGUI>((int)Texts.GoldText).text = Managers.Player.GetGold().ToString();
        Get<TextMeshProUGUI>((int)Texts.ZamText).text = Managers.Player.GetZam().ToString();
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBack);
        GetButton((int)Buttons.ResearchButton).gameObject.BindEvent(OnClickResearch);

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
        Get<TextMeshProUGUI>((int)Texts.GoldText).text = Util.FormatNumber(Managers.Player.GetGold());
        Get<TextMeshProUGUI>((int)Texts.ZamText).text = Util.FormatNumber(Managers.Player.GetZam());
    }

    public void OnClickBack(PointerEventData data)
    {
        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick, this.transform.position);
        Get<GameObject>((int)ReseachObject.UI_Research).SetActive(false);
        Get<Button>((int)Buttons.BackButton).gameObject.SetActive(false);
        Get<Button>((int)Buttons.ResearchButton).gameObject.SetActive(true);
    }

    public void OnClickResearch(PointerEventData data)
    {
        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick, this.transform.position);
        Get<GameObject>((int)ReseachObject.UI_Research).SetActive(true);
        Get<Button>((int)Buttons.BackButton).gameObject.SetActive(true);
        Get<Button>((int)Buttons.ResearchButton).gameObject.SetActive(false);
    }


}
