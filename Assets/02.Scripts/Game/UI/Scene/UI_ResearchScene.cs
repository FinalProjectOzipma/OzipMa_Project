using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using Unity.VisualScripting;

public class UI_ResearchScene : UI_Popup
{
    [SerializeField] private Button BackButton;

    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI ZamText;

    [SerializeField] private Image BackImage;

    [SerializeField] private GameObject UI_Research;

    bool isButton = false;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(UI_Research.transform.DOScale(1.1f,0.1f));
        uiSeq.Append(UI_Research.transform.DOScale(1.0f, 0.1f));

        uiSeq.Play();
    }



    public override void Init()
    {
        GoldText.text = Managers.Player.GetGold().ToString();
        ZamText.text = Managers.Player.GetZam().ToString();
        BackButton.gameObject.BindEvent(OnClickBack);

    }

    private void OnEnable()
    {
        if(uiSeq != null)
        {
            uiSeq = Util.RecyclableSequence();

            uiSeq.Append(UI_Research.transform.DOScale(1.1f, 0.1f));
            uiSeq.Append(UI_Research.transform.DOScale(1.0f, 0.1f));

            uiSeq.Play();
        }

        if (Managers.Player != null)
        {
            Managers.Player.OnGoldChanged += UpdateGoldUI;
            Managers.Player.OnZamChanged += UpdateZamUI;
            UpdateGoldUI(Managers.Player.GetGold());
            UpdateZamUI(Managers.Player.GetZam());
        }
    }

    private void OnDisable()
    {
        if (Managers.Player != null)
        {
            Managers.Player.OnGoldChanged -= UpdateGoldUI;
            Managers.Player.OnZamChanged -= UpdateZamUI;
        }

    }

    private void UpdateGoldUI(long gold)
    {
        GoldText.text = Util.FormatNumber(gold);
    }

    private void UpdateZamUI(long zam)
    {
        ZamText.text = Util.FormatNumber(zam);
    }

    public void OnClickBack(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);

        HidePpoup();
        isButton = false;

    }

    private void HidePpoup()
    {
        uiSeq = Util.RecyclableSequence();

        uiSeq.Append(UI_Research.transform.DOScale(1.1f, 0.1f));
        uiSeq.Append(UI_Research.transform.DOScale(0.2f, 0.1f));

        uiSeq.Play().OnComplete(() =>
        {
            ClosePopupUI();
        });
    }

}
