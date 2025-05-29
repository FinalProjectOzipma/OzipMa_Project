using System;
using UnityEngine;

public class AudioManager
{

    [Range(0, 1)] public float masterVolume = 1f; // 마스터 볼륨
    [Range(0, 1)] public float bgmVolume = 1f; // BGM 볼륨
    [Range(0, 1)] public float sfxVolume = 1f; // SFX 볼륨

    public AudioController audioControler;

    public void Initialize()
    {
        Managers.Resource.Instantiate("AudioMaker", go =>
        {
            go.transform.SetParent(Managers.Instance.transform);
            audioControler = go.GetComponent<AudioController>();
            audioControler.Initialize();
            audioControler.audioManager = this;
        });
    }


    /// <summary>
    /// 씬에 따라 BGM 로드해주는 메서드
    /// </summary>
    public void OnSceneLoaded()
    {
        audioControler.PlayBGM(BGMClipName.gameClip1.ToString()); // 씬 이름에 따라 BGM 자동 재생
    }


    public void SelectSFXAttackType(AbilityType type)
    {
        switch (type)
        {
            case AbilityType.None:
                PlaySFX(SFXClipName.Error);
                break;
            case AbilityType.Physical:
                PlaySFX(SFXClipName.Hit);
                break;
            case AbilityType.Magic:
                PlaySFX(SFXClipName.MagicSpark);
                break;
            case AbilityType.Fire:
                PlaySFX(SFXClipName.Fire);
                break;
            case AbilityType.Explosive:
                PlaySFX(SFXClipName.Destroy);
                break;
            case AbilityType.Dark:
                PlaySFX(SFXClipName.Dark);
                break;
        }

    }

    /// <summary>
    ///   효과음 실행, 다른 스크립트에서 가져가서 실행(효과음 이름, 위치) 
    /// </summary>
    public void PlaySFX(SFXClipName enumName)
    {
        string sfxName = enumName.ToString();

        if (!audioControler.sfxDictionary.ContainsKey(sfxName))
        {
            Util.LogWarning($"효과음 이름이 다릅니다. 효과음 이름과 스크립트에서 매개변수명 확인");
            return;
        }

        audioControler.PlaySFX(audioControler.sfxDictionary[sfxName]);
    }

}
