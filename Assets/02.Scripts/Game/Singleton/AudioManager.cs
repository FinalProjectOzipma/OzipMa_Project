using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.VirtualTexturing;

public class AudioManager
{

    [Range(0f, 1f)] public float masterVolume = 1f; // 마스터 볼륨
    [Range(0f, 1f)] public float bgmVolume = 1f; // BGM 볼륨
    [Range(0f, 1f)] public float sfxVolume = 1f; // SFX 볼륨

    public AudioControler audioControler; // 
    public GameObject sfxPrefab; // 효과음 재생을 위한 오브젝트 프리팹

    public void Initialize()
    {
        Managers.Resource.Instantiate("AudioControler", go =>
        {

            go.transform.SetParent(Managers.Instance.transform);
            audioControler = go.GetComponent<AudioControler>();
            audioControler.Initialize();
            audioControler.audioManager = this;

        });


        Managers.Resource.LoadAssetAsync<GameObject>("AudioSource", go =>
        {
            sfxPrefab = go;
            audioControler.sfxPrefab = sfxPrefab; // 오이도 매니저
            audioControler.InitSFXPool();
        });


    }


}
