using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Alarm : UI_Popup
{
    enum Buttons
    {
        AlarmButton
    }

    enum Texts
    {
        AlarmText,
        ButtonText
    }

    enum Images
    {
        ButtonImage
    }

    enum Objects
    {
        BG
    }

    private bool isClose = false;
    private void Start()
    {
        Init();

        var seq = DOTween.Sequence();

        Managers.Audio.audioControler.PlaySFX(SFXClipName.Error, this.transform.position);

        seq.Append(Get<GameObject>((int)Objects.BG).transform.DOScale(1.1f, 0.1f));
        seq.Append(Get<GameObject>((int)Objects.BG).transform.DOScale(1.0f, 0.1f));

        seq.Play();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(Objects));

        Get<Button>((int)Buttons.AlarmButton).gameObject.BindEvent(CloseAlarmPopup);

    }
    
    public void CloseAlarmPopup(PointerEventData data)
    {
        if (isClose) return;
        isClose = true;

        var seq = DOTween.Sequence();

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick, this.transform.position);

        seq.Append(Get<Image>((int)Images.ButtonImage).transform.DOScale(0.9f, 0.1f));
        seq.Join(Get<TextMeshProUGUI>((int)Texts.ButtonText).transform.DOScale(0.9f, 0.1f));
        seq.Append(Get<Image>((int)Images.ButtonImage).transform.DOScale(1.1f, 0.1f));
        seq.Join(Get<TextMeshProUGUI>((int)Texts.ButtonText).transform.DOScale(1.1f, 0.1f));
        seq.Append(Get<Image>((int)Images.ButtonImage).transform.DOScale(1.0f, 0.1f));
        seq.Join(Get<TextMeshProUGUI>((int)Texts.ButtonText).transform.DOScale(1.0f, 0.1f));


        seq.Play().OnComplete(() =>
        {
            HidePpoup();
        });

    }

    private void HidePpoup()
    {
        var seq = DOTween.Sequence();

        seq.Append(Get<GameObject>((int)Objects.BG).transform.DOScale(1.1f, 0.1f));
        seq.Append(Get<GameObject>((int)Objects.BG).transform.DOScale(0.2f, 0.1f));

        seq.Play().OnComplete(() =>
        {
            ClosePopupUI();
            isClose = false;
        });
    }



}
