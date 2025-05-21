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
        
        AnimePopup(BG);
    }

    private void OnEnable()
    {
        if (uiSeq == null) return;

        AnimePopup(BG);
    }

    public override void Init()
    {
        uiSeq = Util.RecyclableSequence();
        AlarmButton.gameObject.BindEvent(CloseAlarmPopup);
    }

    public void CloseAlarmPopup(PointerEventData data)
    {
        if (isClose) return;
        isClose = true;


        Managers.Audio.PlaySFX(SFXClipName.ButtonClick);

        HidePopup();


    }

    private void HidePopup()
    {
        AnimePopup(BG, true);

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
