using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    [Range(0f, 1f)] public float bgmVolume = 1f; // BGM 볼륨
    public bool isBGMMute = false;
    [Range(0f, 1f)] public float sfxVolume = 1f; // SFX 볼륨
    public bool isSFXMute = false;

    public AudioSource bgmSource; // BGM을 재생하는 AudioSource
    public List<AudioClip> bgmClips; // BGM 리스트

    public GameObject sfxPrefab; // 효과음 재생을 위한 오브젝트 프리팹
    public int poolSize = 10;    // 오브젝트 풀 크기
    private Queue<AudioSource> sfxPool = new Queue<AudioSource>(); // 오디오 소스를 저장할 큐 (오브젝트 풀링)
    public List<AudioClip> sfxClips; // 효과음 리스트

    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>(); // 효과음을 찾는 딕셔너리

    public void Initialize()
    {
        //GameObject audioManagerObject = new GameObject("AudioManager");
        //audioManagerObject.AddComponent<AudioManager>();
        //audioManagerObject.transform.SetParent(Managers.Instance.transform);

        GameObject audioManagerObject = new GameObject("AudioManager");

        var audioManager = audioManagerObject.AddComponent<AudioManager>();
        audioManagerObject.AddComponent<AudioSource>();
        audioManager.transform.SetParent(Managers.Instance.transform);


        audioManager.ManualInit(); // Start() 대신 명시적 초기화 호출


    }

    private void Start()
    {
        Managers.Resource.LoadAssetAsync<GameObject>("AudioSource", go =>
        {
            sfxPrefab = go;

            InitSFXPool();  // 오브젝트 풀 초기화
            LoadSFX();      // SFX 데이터를 딕셔너리에 저장

        });
    }

    private void OnEnable()
    {
        Managers.Scene.CurrentScene.InitAction += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Managers.Scene.CurrentScene.InitAction += OnSceneLoaded;
    }

    public void ManualInit()
    {
        bgmClips = new List<AudioClip>();
        sfxClips = new List<AudioClip>();
        bgmSource = GetComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        LoadClipAsset();

    }

    private void InitSFXPool()
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

    // Dictionary에 효과음 파일명으로 Key값 저장
    private void LoadSFX()
    {
        foreach (var clip in sfxClips)
        {
            sfxDictionary[clip.name] = clip;
        }
    }


    private void LoadClipAsset()
    {
        Addressables.LoadAssetsAsync<AudioClip>("BGM", clip =>
        {
            bgmClips.Add(clip);
        }).Completed += handle =>
        {
             OnSceneLoaded();
        };

        Addressables.LoadAssetsAsync<AudioClip>("SFX", clip =>
        {
            sfxClips.Add(clip);
        });
    }


    private void OnSceneLoaded()
    {
        PlayBGM(Managers.Scene.CurrentScene.GetType().Name); // 씬 이름에 따라 BGM 자동 재생
    }


    // BGM Volume 설정 메서드
    // 씬 이름을 가져와 해당 씬에 맞는 BGM 재생, 씬 전환 시 SoundManager.Instance.PlayBGM(sceneName);
    public void PlayBGM(string sceneName)
    {
        AudioClip bgm = GetBGMByScene(sceneName);

        if (bgm == null)
        {
            Debug.LogWarning($"현재 씬에 브금이 없습니다. Scene 이름 확인");
            return;
        }

        if (isBGMMute)
        {
            bgmSource.clip = bgm;
            bgmSource.Play();
        }
        else
        {
            StartCoroutine(FadeInBGM(bgm));
        }
    }

    // BGM 페이드인 효과(기존 BGM 서서히 사라지고 새로운 BGM이 재생됨)
    private IEnumerator FadeInBGM(AudioClip newBGM)
    {
        float startVolume = bgmSource.volume;
        float targetVolume = bgmVolume; // 저장된 BGM 볼륨 값 사용

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

    // 씬 이름에 따라 해당하는 BGM 반환
    private AudioClip GetBGMByScene(string sceneName)
    {
        switch (sceneName)
        {
            case "StartScene":
                return bgmClips[0];
            case "MainScene":
                return bgmClips[1];
            case "GameScene":
                return bgmClips[2];
            default:
                return null;
        }
    }


    // 효과음 실행, 다른 스크립트에서 가져가서 실행(효과음 이름, 위치) 
    public void PlaySFX(string sfxName, Vector3 position)
    {
        if (!sfxDictionary.ContainsKey(sfxName))
        {
            Debug.LogWarning($"효과음 이름이 다릅니다. 효과음 이름과 스크립트에서 매개변수명 확인");
            return;
        }

        PlaySFX(sfxDictionary[sfxName], position);
    }

    private void PlaySFX(AudioClip clip, Vector3 position)
    {
        // 음소거 상태라면 재생 안됨
        if (isSFXMute) return;

        AudioSource sfxSource = GetAudioSource(); // 오디오 소스 가져오기

        sfxSource.transform.position = position; // 재생 위치 설정
        sfxSource.clip = clip;
        sfxSource.volume = isSFXMute ? 0 : sfxVolume; // SFX 볼륨 설정
        sfxSource.gameObject.SetActive(true);
        sfxSource.Play();

        StartCoroutine(ReturnToPool(sfxSource, clip.length + 2.0f)); // 재생 후 다시 풀에 반환
    }

    // Queue에 풀이 없으면 풀 추가
    private AudioSource GetAudioSource()
    {
        if (sfxPool.Count > 0)
        {
            return sfxPool.Dequeue(); // 큐에서 오디오 소스 가져오기
        }

        GameObject sfxObj = Instantiate(sfxPrefab, transform);

        return sfxObj.GetComponent<AudioSource>();
    }

    // 효과음이 끝난 후 다시 풀에 반환
    private IEnumerator ReturnToPool(AudioSource sfxSource, float delay)
    {
        yield return new WaitForSeconds(delay);

        sfxSource.gameObject.SetActive(false); // 오브젝트 비활성화
        sfxPool.Enqueue(sfxSource);            // 큐에 다시 추가
    }

}
