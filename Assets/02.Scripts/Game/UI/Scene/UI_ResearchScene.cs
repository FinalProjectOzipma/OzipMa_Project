using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ResearchScene : UI_Popup
{
    [SerializeField] private Button BGClose;

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
        BGClose.gameObject.BindEvent(OnClickBack);
    }

    private void OnEnable()
    {
        AnimePopup(UI_Research);
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
