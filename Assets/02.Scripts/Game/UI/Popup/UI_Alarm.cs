using DG.Tweening;
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

        uiSeq = Util.RecyclableSequence();

        Managers.Audio.audioControler.PlaySFX(SFXClipName.Error);

        uiSeq.Append(Get<GameObject>((int)Objects.BG).transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(Get<GameObject>((int)Objects.BG).transform.DOScale(1.0f, 0.1f));

        uiSeq.Play();
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


        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);
        
        HidePpoup();
   

    }

    private void HidePpoup()
    {
        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(Get<GameObject>((int)Objects.BG).transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(Get<GameObject>((int)Objects.BG).transform.DOScale(0.2f, 0.1f));

        uiSeq.Play().OnComplete(() =>
        {
            ClosePopupUI();
            isClose = false;
        });
    }



}
