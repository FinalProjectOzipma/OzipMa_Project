using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{

    [SerializeField] private Button SoundButton;
    [SerializeField] private Button LanguageButton;
    [SerializeField] private Button AccountButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private GameObject Setting;

    bool isButton = false;


    private void Start()
    {
        Init();

        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(Setting.transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(Setting.transform.DOScale(1.0f, 0.1f));

        uiSeq.Play();
    }

    private void OnEnable()
    {
        if (uiSeq != null)
        {
            uiSeq = Util.RecyclableSequence();

            uiSeq.Append(Setting.transform.DOScale(1.1f, 0.1f));
            uiSeq.Append(Setting.transform.DOScale(1.0f, 0.1f));

            uiSeq.Play();
        }
    }


    public override void Init()
    {
        SoundButton.onClick.AddListener(OnClickSound);
        BackButton.onClick.AddListener(OnClickBack);
    }


    public void OnClickSound()
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);
        Managers.UI.ShowPopupUI<UI_Sound>("SoundUI");
        isButton = false;
    }


    public void OnClickBack()
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);

        HidePpoup();
    }


    private void HidePpoup()
    {
        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(Setting.transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(Setting.transform.DOScale(0.2f, 0.1f));

        uiSeq.Play().OnComplete(() =>
        {
            ClosePopupUI();
            isButton = false;
        });
    }


}
