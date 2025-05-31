using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Threading.Tasks;

public class UI_Sound : UI_Popup
{
    [SerializeField] private Button MasterMuteButton;
    [SerializeField] private Button BGMBMuteButton;
    [SerializeField] private Button SFMMuteButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button ExitButton;

    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private GameObject OnMaster;
    [SerializeField] private GameObject OffMaster;
    [SerializeField] private GameObject OnBGM;
    [SerializeField] private GameObject OffBGM;
    [SerializeField] private GameObject OnSFM;
    [SerializeField] private GameObject OffSFM;



    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        MasterMuteButton.gameObject.BindEvent(OnClickMasterMuted);
        BGMBMuteButton.gameObject.BindEvent(OnClickBGMMuted);
        SFMMuteButton.gameObject.BindEvent(OnClickSFXMuted);
        BackButton.gameObject.BindEvent(OnClickBack);
        ExitButton.onClick.AddListener(ExitGame);

        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        MasterSlider.onValueChanged.AddListener((v) => OnChangeVolume(AudioType.Master, v));
        BGMSlider.onValueChanged.AddListener((v) => OnChangeVolume(AudioType.BGM, v));
        SFXSlider.onValueChanged.AddListener((v) => OnChangeVolume(AudioType.SFX, v));


        InitVolumeMuted();

    }


    #region 볼륨 조절 설정

    public void OnChangeVolume(AudioType type, float value)
    {
        float dB;

        bool isMute;

        switch (type)
        {
            case AudioType.Master:
                isMute = Managers.Audio.audioControler.isMasterMute;
                Managers.Audio.masterVolume = value;
                break;
            case AudioType.BGM:
                isMute = Managers.Audio.audioControler.isBGMMute;
                Managers.Audio.bgmVolume = value;
                break;
            case AudioType.SFX:
                isMute = Managers.Audio.audioControler.isSFXMute;
                Managers.Audio.sfxVolume = value;
                break;
            default:
                isMute = false;
                break;
        }

        if (value <= 0.0001f || isMute)
            dB = -80.0f;
        else
            dB = Mathf.Log10(value) * 20f;

        Managers.Audio.audioControler.SetVolume(type, dB);

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
            OnMaster.SetActive(false);
            OffMaster.SetActive(true);
        }
        else
        {
            OnMaster.SetActive(true);
            OffMaster.SetActive(false);
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
            OnBGM.SetActive(false);
            OffBGM.SetActive(true);
        }
        else
        {
            OnBGM.SetActive(true);
            OffBGM.SetActive(false);
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
            OnSFM.SetActive(false);
            OffSFM.SetActive(true);
        }
        else
        {
            OnSFM.SetActive(true);
            OffSFM.SetActive(false);
        }
    }
    #endregion



    public void InitVolumeMuted()
    {
        if (Managers.Audio.audioControler.isMasterMute)
        {
            OnMaster.SetActive(false);
            OffMaster.SetActive(true);
        }
        else
        {
            OnMaster.SetActive(true);
            OffMaster.SetActive(false);
        }

        if (Managers.Audio.audioControler.isBGMMute)
        {
            OnBGM.SetActive(false);
            OffBGM.SetActive(true);
        }
        else
        {
            OnBGM.SetActive(true);
            OffBGM.SetActive(false);
        }

        if (Managers.Audio.audioControler.isSFXMute)
        {
            OnSFM.SetActive(false);
            OffSFM.SetActive(true);
        }
        else
        {
            OnSFM.SetActive(true);
            OffSFM.SetActive(false);
        }
    }


    /// <summary>
    /// 나가기 버튼
    /// </summary>
    public void OnClickBack(PointerEventData data)
    {
        bool isButton = false;

        if (isButton) return;

        isButton = true;

        Managers.Audio.PlaySFX(SFXClipName.ButtonClick);

        ClosePopupUI();
    }

    public async void ExitGame()
    {
        try
        {
            // 서버 시간 기준 종료 시간 저장
            Managers.Player.RewordStartTime = Managers.Game.ServerUtcNow.ToString("o");

            // 비동기 저장 호출 (SaveGameDataAsync는 Task 반환)
            await Managers.Data.SaveGameDataAsync();

            // 저장 여유 시간 확보 (선택 사항)
            await Task.Delay(1000);

            // 정상 종료
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        catch (Exception ex)
        {
            Util.LogWarning($"ExitGame 저장 실패: {ex.Message}");
            // 그래도 종료 시도
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }

}
