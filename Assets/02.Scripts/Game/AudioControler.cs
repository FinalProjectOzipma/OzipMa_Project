using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


public enum BGMClipName
{
    gameClip1,
    gameClip2,
    normalClip
}

public enum SFXClipName
{
    ButtonClick,
    Sword,
    MagicSpark,
    Hit,
    Destroy,
    Buff,
    Thunder,
    Walk,
    Dead,
    PowerUp,
    Error,
    Upgrade
}

public class AudioControler : MonoBehaviour
{
    public AudioManager audioManager; // 오디오 매니저
    public BGMData bgmData; // BGM 데이터
    public SFXData sfxData; // SFX 데이터

    // 음소거 버튼을 위한 불값
    public bool isMasterMute = false;
    public bool isBGMMute = false;
    public bool isSFXMute = false;

    public AudioSource bgmSource; // BGM을 재생하는 AudioSource
    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>(); // bgmData 데이터 저장할 딕셔너리

    public GameObject sfxPrefab; // 효과음 재생을 위한 오브젝트 프리팹
    public int poolSize = 10;    // 초기 sfxPrefab 오브젝트 풀 크기
    private Queue<AudioSource> sfxPool = new Queue<AudioSource>(); // sfx 오디오 소스를 저장할 큐 (오브젝트 풀링)
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>(); // sfxData 데이터 저장할 딕셔너리


    /// <summary>
    /// 오디오 컨트롤러 초기화 해주는 메서드
    /// </summary>
    public void Initialize()
    {
        bgmSource = this.GetComponent<AudioSource>();
        BuildBGMDictionary(); // bgmData를 딕셔너리 형태로 변환
        BuildSFXictionary(); // sfxData를 딕셔너리 형태로 변환
        InitSFXPool(); // sfx 오브젝트 풀 초기화
        LoadVolumes(); // 저장된 볼륨 값 로드
        LoadMuteSettings(); // 저장된 음소거 설정 로드
    }

    private void Start()
    {
        OnSceneLoaded();
    }

    /// <summary>
    /// 씬에 따라 BGM 로드해주는 메서드
    /// </summary>
    private void OnSceneLoaded()
    {
        PlayBGM(BGMClipName.gameClip1.ToString()); // 씬 이름에 따라 BGM 자동 재생
    }

    #region 오디오 초기화 설정
    /// <summary>
    /// bgmData에 오디오 클립들을 딕셔너리에 저장할 매서드
    /// </summary>
    private void BuildBGMDictionary()
    {
        bgmDictionary.Clear();
        foreach (var entry in bgmData.entries)
        {
            if (!string.IsNullOrEmpty(entry.key) && entry.clip != null)
            {
                bgmDictionary[entry.key] = entry.clip;
            }
        }
    }

    /// <summary>
    /// sfxData에 오디오 클립들을 딕셔너리에 저장할 매서드
    /// </summary>
    private void BuildSFXictionary()
    {
        sfxDictionary.Clear();
        foreach (var entry in sfxData.entries)
        {
            if (!string.IsNullOrEmpty(entry.key) && entry.clip != null)
            {
                sfxDictionary[entry.key] = entry.clip;
            }
        }
    }


    /// <summary>
    /// sfx 오디오 소스 초기화 해주는 매서드
    /// </summary>
    public void InitSFXPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject sfxObj = GameObject.Instantiate(sfxPrefab, transform);
            //sfxObj.name = sfxObj.name + $"{i}";
            AudioSource sfxSource = sfxObj.GetComponent<AudioSource>();
            sfxObj.SetActive(false);
            sfxPool.Enqueue(sfxSource);
        }
    }
    #endregion

    #region BGM 관련 메서드
    /// <summary>
    /// 씬 이름을 가져와 해당 씬에 맞는 BGM 재생, 씬 전환 시 SoundManager.Instance.PlayBGM(sceneName);
    /// </summary>
    public void PlayBGM(string sceneName)
    {
        AudioClip bgm = bgmDictionary[sceneName];
            //GetBGMByScene(sceneName);

        if (bgm == null)
        {
            Debug.LogWarning($"현재 씬에 브금이 없습니다. Scene 이름 확인");
            return;
        }

        if (!isBGMMute || !isMasterMute)
        {
            bgmSource.clip = bgm;
            bgmSource.Play();
        }
        else
        {
            StartCoroutine(FadeInBGM(bgm));
        }
    }


    /// <summary>
    ///  BGM 페이드인 효과(기존 BGM 서서히 사라지고 새로운 BGM이 재생됨)
    /// </summary>
    private IEnumerator FadeInBGM(AudioClip newBGM)
    {
        float startVolume = bgmSource.volume;
        float targetVolume = Managers.Audio.bgmVolume; // 저장된 BGM 볼륨 값 사용

        // 기존 BGM 서서히 감소
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            if (isBGMMute)
            {
                bgmSource.volume = 0;
                break;
            }
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t);
            yield return null;
        }

        // 새로운 BGM 변경 후 실행
        bgmSource.clip = newBGM;
        bgmSource.Play();

        // 볼륨을 저장된 값으로 서서히 증가
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            if (isBGMMute)
            {
                bgmSource.volume = 0;
                yield break;
            }

            bgmSource.volume = Mathf.Lerp(0, targetVolume, t);
            yield return null;
        }

        // 최종 볼륨 설정
        if (!isBGMMute) bgmSource.volume = targetVolume;
    }
    #endregion

    #region SFX 관련 메서드
    /// <summary>
    ///   효과음 실행, 다른 스크립트에서 가져가서 실행(효과음 이름, 위치) 
    /// </summary>
    public void PlaySFX(SFXClipName enumName, Vector3 position)
    {
        string sfxName = enumName.ToString();

        if (!sfxDictionary.ContainsKey(sfxName))
        {
            Debug.LogWarning($"효과음 이름이 다릅니다. 효과음 이름과 스크립트에서 매개변수명 확인");
            return;
        }

        PlaySFX(sfxDictionary[sfxName], position);
    }


    /// <summary>
    /// 다른곳에서 받은 효과음 실행
    /// </summary>
    private void PlaySFX(AudioClip clip, Vector3 position)
    {
        // 음소거 상태라면 재생 안됨
        if (isSFXMute) return;

        AudioSource sfxSource = GetAudioSource(); // 오디오 소스 가져오기

        sfxSource.transform.position = position; // 재생 위치 설정
        sfxSource.clip = clip;

        if(isMasterMute || isSFXMute)
        {
            sfxSource.volume = 0;
        }
        else
        {
            sfxSource.volume = Managers.Audio.sfxVolume * Managers.Audio.masterVolume; // SFX 볼륨 설정
        }

        sfxSource.gameObject.SetActive(true);
        sfxSource.Play();

        StartCoroutine(ReturnToPool(sfxSource, clip.length + 2.0f)); // 재생 후 다시 풀에 반환
    }

    /// <summary>
    /// Queue에 풀이 없으면 풀 추가
    /// </summary>
    private AudioSource GetAudioSource()
    {
        if (sfxPool.Count > 0)
        {
            return sfxPool.Dequeue(); // 큐에서 오디오 소스 가져오기
        }

        GameObject sfxObj = Instantiate(sfxPrefab, transform);

        return sfxObj.GetComponent<AudioSource>();
    }

    /// <summary>
    /// 효과음이 끝난 후 다시 풀에 반환
    /// </summary
    private IEnumerator ReturnToPool(AudioSource sfxSource, float delay)
    {
        yield return new WaitForSeconds(delay);

        sfxSource.gameObject.SetActive(false); // 오브젝트 비활성화
        sfxPool.Enqueue(sfxSource);            // 큐에 다시 추가
    }
    #endregion

    #region 사운드 UI와 연결되는 메서드

    /// <summary>
    /// 마스터 볼륨 세팅
    /// </summary
    public void SetMasterVolume(float volume)
    {
        Managers.Audio.masterVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
        SaveVolumes();
    }

    /// <summary>
    /// BGM 볼륨 세팅
    /// </summary
    public void SetBGMVolume(float volume)
    {
        Managers.Audio.bgmVolume = Mathf.Clamp01(volume);
        UpdateVolumes();
        SaveVolumes();
    }

    /// <summary>
    /// SFX 볼륨 세팅
    /// </summary
    public void SetSFXVolume(float volume)
    {
        Managers.Audio.sfxVolume = Mathf.Clamp01(volume);
        SaveVolumes();
    }

    /// <summary>
    /// 마스터 볼륨 음소거 설정
    /// </summary
    public void ToggleMasterMute()
    {
        isMasterMute = !isMasterMute;
        UpdateVolumes();
        SaveMuteSettings();
    }

    /// <summary>
    /// BGM 볼륨 음소거 설정
    /// </summary
    public void ToggleBGMMute()
    {
        isBGMMute = !isBGMMute;
        UpdateVolumes();
        SaveMuteSettings();
    }

    /// <summary>
    /// SFX 볼륨 음소거 설정
    /// </summary
    public void ToggleSFXMute()
    {
        isSFXMute = !isSFXMute;
        SaveMuteSettings();
    }


    /// <summary>
    /// 볼륨 값 저장
    /// </summary
    private void SaveVolumes()
    {
        PlayerPrefs.SetFloat("MasterVolume", Managers.Audio.masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", Managers.Audio.bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", Managers.Audio.sfxVolume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 저장된 볼륨 로드
    /// </summary
    private void LoadVolumes()
    {
        Managers.Audio.masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        Managers.Audio.bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        Managers.Audio.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        UpdateVolumes();
    }

    /// <summary>
    /// 현재 설정된 음소거 설정 저장
    /// </summary
    private void SaveMuteSettings()
    {
        PlayerPrefs.SetInt("MasterMuted", isMasterMute ? 1 : 0);
        PlayerPrefs.SetInt("BGMMuted", isBGMMute ? 1 : 0);
        PlayerPrefs.SetInt("SFXMuted", isSFXMute ? 1 : 0);
        PlayerPrefs.Save();
    }


    /// <summary>
    /// 현재 설정된 음소거 설정 로드
    /// </summary
    private void LoadMuteSettings()
    {
        isMasterMute = PlayerPrefs.GetInt("MasterMuted", 0) == 1;
        isBGMMute = PlayerPrefs.GetInt("BGMMuted", 0) == 1;
        isSFXMute = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        UpdateVolumes();
    }

    /// <summary>
    /// BGM 업데이트 매서드
    /// </summary
    public void UpdateVolumes()
    {
        if (bgmSource != null)
        {
            if(isMasterMute || isBGMMute)
            {
                bgmSource.volume = 0;
            }
            else
            {
                bgmSource.volume = Managers.Audio.bgmVolume * Managers.Audio.masterVolume;
            }
        }
    }
    #endregion




}
