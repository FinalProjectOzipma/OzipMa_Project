using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.Numerics;
using UnityEngine;

public class UI_ResearchScene : UI_Popup
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

    enum Images
    {
        BackImage
    }

    enum ReseachObject
    {
        UI_Research
    }

    bool isButton = false;

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
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick, this.transform.position);

        var seq = DOTween.Sequence();

        seq.Append(Get<Image>((int)Images.BackImage).transform.DOScale(0.9f, 0.1f));
        seq.Append(Get<Image>((int)Images.BackImage).transform.DOScale(1.1f, 0.1f));
        seq.Append(Get<Image>((int)Images.BackImage).transform.DOScale(1.0f, 0.1f));

        seq.Play().OnComplete(() =>
        {
            ClosePopupUI();
            isButton = false;
        });
    }

}
