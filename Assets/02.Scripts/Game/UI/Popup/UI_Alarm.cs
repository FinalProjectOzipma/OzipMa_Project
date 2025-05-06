using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Alarm : UI_Popup
{
    [SerializeField] private Button AlarmButton;

    [SerializeField] private TextMeshProUGUI AlarmText;
    [SerializeField] private TextMeshProUGUI ButtonText;

    [SerializeField] private Image ButtonImage;

    [SerializeField] private GameObject BG;

    private bool isClose = false;
    private void Start()
    {
        Init();

        uiSeq = Util.RecyclableSequence();

        Managers.Audio.audioControler.PlaySFX(SFXClipName.Error);

        uiSeq.Append(BG.transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(BG.transform.DOScale(1.0f, 0.1f));

        uiSeq.Play();
    }

    public override void Init()
    {
        base.Init();

        AlarmButton.gameObject.BindEvent(CloseAlarmPopup);

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

        uiSeq.Append(BG.transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(BG.transform.DOScale(0.2f, 0.1f));

        uiSeq.Play().OnComplete(() =>
        {
            ClosePopupUI();
            isClose = false;
        });
    }

    public void WriteText(string text)
    {
        AlarmText.text = text.ToString();
    }
}
