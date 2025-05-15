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
    [SerializeField] private Image ManagerButtonImage;
    [SerializeField] private Image DictionaryButtonImage;
    [SerializeField] private Image ResearchButtonImage;
    [SerializeField] private Image GachaButtonImage;
    [SerializeField] private Image SettingImage;

    [SerializeField] private GameObject OFFManagerBtn;
    [SerializeField] private GameObject ONManagerBtn;
    [SerializeField] private GameObject OFFDictionaryBtn;
    [SerializeField] private GameObject ONDictionaryBtn;
    [SerializeField] private GameObject OFFResearchBtn;
    [SerializeField] private GameObject ONResearchBtn;
    [SerializeField] private GameObject OFFGachaBtn;
    [SerializeField] private GameObject ONGachaBtn;


    public bool isManagerOpen = false;
    bool isDictionaryOpne = false;
    bool isResearchOpne = false;
    bool isGachaOpne = false;


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
        base.Init();
        Managers.UI.SetSceneList<UI_Main>(this);

        MainGoldText.text = Util.FormatNumber(Managers.Player.GetGold());
        MainZamText.text = Util.FormatNumber(Managers.Player.GetZam());
        ResearchButton.gameObject.BindEvent(OnClikButtonResearch);
        ManagerButton.gameObject.BindEvent(OnClickManager);
        SettingButton.gameObject.BindEvent(OnClickSetting);
        GachaButton.gameObject.BindEvent(OnClickGacha);
        DictionaryButton.gameObject.BindEvent(OnClickDictionary);
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


    public void OnClikButtonResearch(PointerEventData data)
    {
        if (isButton) return;
        isButton = true;

        if (!isResearchOpne)
        {
            AllOFF();
            OFFSwipe();
            Managers.UI.CloseAllPopupUI();
            ONResearchBtn.SetActive(true);
            Managers.UI.ShowPopupUI<UI_ResearchScene>(Objects.ReseachUI.ToString());
            isResearchOpne = true;
        }
        else
        {
            AllOFF();
            Managers.UI.CloseAllPopupUI();
        }

        Managers.Audio.PlaySFX(SFXClipName.ButtonClick);
        isButton = false;
    }

    private void OnClickSetting(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        Managers.Audio.PlaySFX(SFXClipName.ButtonClick);
        Managers.UI.ShowPopupUI<UI_Sound>("SoundUI");
        isButton = false;
    }

    private void OnClickManager(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        AllOFF();
        Managers.UI.CloseAllPopupUI();
        Managers.UI.GetScene<InventoryUI>().OnSwipe();
        Managers.Audio.PlaySFX(SFXClipName.ButtonClick);
        isButton = false;

    }

    public void OnClickGacha(PointerEventData data)
    {
        if (isButton) return;

        isButton = true;

        if (!isGachaOpne)
        {
            AllOFF();
            OFFSwipe();
            Managers.UI.CloseAllPopupUI();

            ONGachaBtn.SetActive(true);
            Managers.UI.ShowPopupUI<GachaUI>(nameof(GachaUI));
            isGachaOpne = true;
        }
        else
        {
            AllOFF();
            Managers.UI.CloseAllPopupUI();
        }


        Managers.Audio.PlaySFX(SFXClipName.ButtonClick);
        isButton = false;
    }

    public void OnClickDictionary(PointerEventData data)
    {
        if (isButton) return;
        isButton = true;

        if (!isDictionaryOpne)
        {
            AllOFF();
            OFFSwipe();
            Managers.UI.CloseAllPopupUI();         

            ONDictionaryBtn.SetActive(true);
            Managers.UI.ShowPopupUI<UI_Dictionary>("DictionaryUI");
            isDictionaryOpne = true;
        }
        else
        {
            AllOFF();
            Managers.UI.CloseAllPopupUI();     
        }

        Managers.Audio.PlaySFX(SFXClipName.ButtonClick);

        isButton = false;

    }

    private void AllOFF()
    {

        OFFDictionaryBtn.SetActive(true);
        OFFResearchBtn.SetActive(true);
        OFFGachaBtn.SetActive(true);

        ONDictionaryBtn.SetActive(false);
        ONResearchBtn.SetActive(false);
        ONGachaBtn.SetActive(false);

        isDictionaryOpne = false;
        isResearchOpne = false;
        isGachaOpne = false;
    }

    public void OnManagerMenu()
    {
        isManagerOpen = true;
        OFFManagerBtn.SetActive(false);
        ONManagerBtn.SetActive(true);
    }

    public void OFFManagerMenu()
    {
        isManagerOpen = false;
        OFFManagerBtn.SetActive(true);
        ONManagerBtn.SetActive(false);
    }

    private void OFFSwipe()
    {
        if (isManagerOpen) Managers.UI.GetScene<InventoryUI>().OnSwipe();
    }

}
