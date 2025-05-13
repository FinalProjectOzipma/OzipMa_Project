using DefaultTable;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor.SceneManagement;



public class UI_Main : UI_Scene
{
    [SerializeField] private Button ResearchButton;
    [SerializeField] private Button ManagerButton;
    [SerializeField] private Button SettingButton;
    [SerializeField] private Button DictionaryButton;
    [SerializeField] private Button GachaButton;

    [SerializeField] private TextMeshProUGUI MainGoldText;
    [SerializeField] private TextMeshProUGUI MainZamText;
    [SerializeField] private TextMeshProUGUI StageLv;
    [SerializeField] private TextMeshProUGUI PlayerName;
    [SerializeField] private TextMeshProUGUI ResearchText;
    [SerializeField] private TextMeshProUGUI ManagerText;
    [SerializeField] private TextMeshProUGUI DictionaryText;

    [SerializeField] private Image ProfileImage;
    [SerializeField] private Image ResearchButtonImage;
    [SerializeField] private Image ManagerButtonImage;
    [SerializeField] private Image SettingImage;
    [SerializeField] private Image ProgressImage;
    [SerializeField] private Image CompleteImage;
    [SerializeField] private Image DictionaryButtonImage;


    enum Objects
    {
        ReseachUI,
        SoundUI,
    }


    bool isButton = false;


    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        Managers.UI.SetSceneList<UI_Main>(this);

        MainGoldText.text = Util.FormatNumber(Managers.Player.GetGold());
        MainZamText.text = Util.FormatNumber(Managers.Player.GetZam());
        ResearchButton.gameObject.BindEvent(OnClikButtonResearch);
        ManagerButton.gameObject.BindEvent(OnClickManager);
        SettingButton.gameObject.BindEvent(OnClickSetting);
        GachaButton.gameObject.BindEvent(OnClickGacha);
        StageLv.text = $"Lv {Managers.Player.CurrentStage} - {Managers.Player.CurrentWave + 1}";

        if (Managers.Player != null)
        {
            Managers.Player.OnGoldChanged += UpdateGoldUI;
            Managers.Player.OnZamChanged += UpdateZamUI;
            Managers.Player.OnStageChanged += UpdateStageUI;
            UpdateGoldUI(Managers.Player.GetGold());
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
        MainGoldText.text = Util.FormatNumber(gold);
    }

    private void UpdateZamUI(long zam)
    {
        MainZamText.text = Util.FormatNumber(zam);
    }

    private void UpdateStageUI(int stage, int wave)
    {
        StageLv.text = $"Lv {stage} - {wave + 1}";
    }


    private void OnClikButtonResearch(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);
        Managers.UI.ShowPopupUI<UI_ResearchScene>(Objects.ReseachUI.ToString());
        isButton = false;
    }

    private void OnClickSetting(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);
        Managers.UI.ShowPopupUI<UI_Setting>("SettingUI");
        isButton = false;
    }

    private void OnClickManager(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);
        Managers.UI.GetScene<InventoryUI>().OnSwipe();
        isButton = false;
    }

    private void OnClickGacha(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.audioControler.PlaySFX(SFXClipName.ButtonClick);
        Managers.UI.ShowPopupUI<GachaUI>(nameof(GachaUI));
        isButton = false;
    }

    public void OffButton()
    {
        ManagerButton.gameObject.SetActive(false);
        ResearchButton.gameObject.SetActive(false);
        DictionaryButton.gameObject.SetActive(false);
        GachaButton.gameObject.SetActive(false);
    }

    public void OnButton()
    {
        ManagerButton.gameObject.SetActive(true);
        ResearchButton.gameObject.SetActive(true);
        DictionaryButton.gameObject.SetActive(true);
        GachaButton.gameObject.SetActive(true);
    }
}
