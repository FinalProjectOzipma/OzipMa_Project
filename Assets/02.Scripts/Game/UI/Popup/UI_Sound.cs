using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Sound : UI_Base
{
    [SerializeField] private Button MasterMuteButton;
    [SerializeField] private Button BGMBMuteButton;
    [SerializeField] private Button SFMMuteButton;

    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;


    private void Awake()
    {
        Init();
    }



    public override void Init()
    {



        MasterMuteButton.gameObject.BindEvent(OnClickMasterMuted);
        BGMBMuteButton.gameObject.BindEvent(OnClickBGMMuted);
        SFMMuteButton.gameObject.BindEvent(OnClickSFXMuted);

        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        MasterSlider.onValueChanged.AddListener(OnChangeMasterVolume);
        BGMSlider.onValueChanged.AddListener(OnChangeBGMVolume);
        SFXSlider.onValueChanged.AddListener(OnChangeSFXVolume);

        InitVolumeMuted();

    }

    #region 볼륨 조절 설정
    public void OnChangeMasterVolume(float value)
    {
        Managers.Audio.audioControler.SetMasterVolume(value);
    }

    public void OnChangeBGMVolume(float value)
    {
        Managers.Audio.audioControler.SetBGMVolume(value);
    }

    public void OnChangeSFXVolume(float value)
    {
        Managers.Audio.audioControler.SetSFXVolume(value);
    }
    #endregion

    #region 음소거 설정
    /// <summary>
    /// 마스터 볼륨 음소거
    /// </summary>
    public void OnClickMasterMuted(PointerEventData data)
    {
        Managers.Audio.audioControler.ToggleMasterMute();

        if (Managers.Audio.audioControler.isMasterMute)
        {
            MasterMuteButton.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            MasterMuteButton.GetComponent<Image>().color = Color.white;
        }

    }

    /// <summary>
    /// BGM 볼륨 음소거
    /// </summary>
    public void OnClickBGMMuted(PointerEventData data)
    {
        Managers.Audio.audioControler.ToggleBGMMute();

        if (Managers.Audio.audioControler.isBGMMute)
        {
            BGMBMuteButton.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            BGMBMuteButton.GetComponent<Image>().color = Color.white;
        }
    }

    /// <summary>
    /// SFX 볼륨 음소거
    /// </summary>
    public void OnClickSFXMuted(PointerEventData data)
    {
        Managers.Audio.audioControler.ToggleSFXMute();

        if (Managers.Audio.audioControler.isSFXMute)
        {
            SFMMuteButton.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            SFMMuteButton.GetComponent<Image>().color = Color.white;
        }
    }
    #endregion

    /// <summary>
    /// 나가기 버튼
    /// </summary>
 
    public void InitVolumeMuted()
    {
        if (Managers.Audio.audioControler.isMasterMute)
        {
            MasterMuteButton.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            MasterMuteButton.GetComponent<Image>().color = Color.white;
        }

        if (Managers.Audio.audioControler.isBGMMute)
        {
            BGMBMuteButton.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            BGMBMuteButton.GetComponent<Image>().color = Color.white;
        }

        if (Managers.Audio.audioControler.isSFXMute)
        {
            SFMMuteButton.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            SFMMuteButton.GetComponent<Image>().color = Color.white;
        }


    }
}
