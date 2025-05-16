using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ResearchScene : UI_Popup
{
    [SerializeField] private Button BGClose;

    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI ZamText;

    [SerializeField] private Image BackImage;

    [SerializeField] public GameObject UI_Research;

    bool isButton = false;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        AnimePopup(UI_Research);
    }

    public override void Init()
    {
        GoldText.text = Managers.Player.GetGold().ToString();
        ZamText.text = Managers.Player.GetZam().ToString();
        BGClose.gameObject.BindEvent(OnClickBack);

    }

    private void OnEnable()
    {
        AnimePopup(UI_Research);

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

        Managers.Audio.PlaySFX(SFXClipName.ButtonClick);

        HidePpoup(data);
        isButton = false;

    }

    private void HidePpoup(PointerEventData data)
    {
        Managers.UI.GetScene<UI_Main>().OnClikButtonResearch(data);
    }

}
