using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    [SerializeField] private Button BackButton;
    [SerializeField] private Image BackImage;
    [SerializeField] private GameObject UI_Sound;

    bool isButton = false;


    private void Start()
    {
        Init();

        var seq = DOTween.Sequence();

        seq.Append(UI_Sound.transform.DOScale(1.1f, 0.1f));
        seq.Append(UI_Sound.transform.DOScale(1.0f, 0.1f));

        seq.Play();

    }


    public override void Init()
    {
        BackButton.gameObject.BindEvent(OnClickBack);

    }

    public void OnClickBack(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);
   
        var seq = DOTween.Sequence();

        //seq.Append(Get<Image>((int)Images.BackImage).transform.DOScale(0.9f, 0.1f));
        //seq.Append(Get<Image>((int)Images.BackImage).transform.DOScale(1.1f, 0.1f));
        //seq.Append(Get<Image>((int)Images.BackImage).transform.DOScale(1.0f, 0.1f));

        seq.Play().OnComplete(() =>
        {
            HidePpoup();
            isButton = false;
        });
    }


    private void HidePpoup()
    {
        var seq = DOTween.Sequence();

        seq.Append(UI_Sound.transform.DOScale(1.1f, 0.1f));
        seq.Append(UI_Sound.transform.DOScale(0.2f, 0.1f));

        seq.Play().OnComplete(() =>
        {
            ClosePopupUI();
        });
    }


}
