using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Sound : UI_Base
{
    enum Buttons
    {
        MasterMuteButton,
        BGMBMuteButton,
        SFMMuteButton
    }

    enum Sliders
    {
        MasterSlider,
        BGMSlider,
        SFXSlider
    }


    private void Awake()
    {
        Init();
    }



    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Slider>(typeof(Sliders));


        GetButton((int)Buttons.MasterMuteButton).gameObject.BindEvent(OnClickMasterMuted);
        GetButton((int)Buttons.BGMBMuteButton).gameObject.BindEvent(OnClickBGMMuted);
        GetButton((int)Buttons.SFMMuteButton).gameObject.BindEvent(OnClickSFXMuted);

        Get<Slider>((int)Sliders.MasterSlider).value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        Get<Slider>((int)Sliders.BGMSlider).value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        Get<Slider>((int)Sliders.SFXSlider).value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        Get<Slider>((int)Sliders.MasterSlider).onValueChanged.AddListener(OnChangeMasterVolume);
        Get<Slider>((int)Sliders.BGMSlider).onValueChanged.AddListener(OnChangeBGMVolume);
        Get<Slider>((int)Sliders.SFXSlider).onValueChanged.AddListener(OnChangeSFXVolume);

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
            GetButton((int)Buttons.MasterMuteButton).GetComponent<Image>().color = Color.gray;
        }
        else
        {
            GetButton((int)Buttons.MasterMuteButton).GetComponent<Image>().color = Color.white;
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
            GetButton((int)Buttons.BGMBMuteButton).GetComponent<Image>().color = Color.gray;
        }
        else
        {
            GetButton((int)Buttons.BGMBMuteButton).GetComponent<Image>().color = Color.white;
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
            GetButton((int)Buttons.SFMMuteButton).GetComponent<Image>().color = Color.gray;
        }
        else
        {
            GetButton((int)Buttons.SFMMuteButton).GetComponent<Image>().color = Color.white;
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
            GetButton((int)Buttons.MasterMuteButton).GetComponent<Image>().color = Color.gray;
        }
        else
        {
            GetButton((int)Buttons.MasterMuteButton).GetComponent<Image>().color = Color.white;
        }

        if (Managers.Audio.audioControler.isBGMMute)
        {
            GetButton((int)Buttons.BGMBMuteButton).GetComponent<Image>().color = Color.gray;
        }
        else
        {
            GetButton((int)Buttons.BGMBMuteButton).GetComponent<Image>().color = Color.white;
        }

        if (Managers.Audio.audioControler.isSFXMute)
        {
            GetButton((int)Buttons.SFMMuteButton).GetComponent<Image>().color = Color.gray;
        }
        else
        {
            GetButton((int)Buttons.SFMMuteButton).GetComponent<Image>().color = Color.white;
        }


    }
}
