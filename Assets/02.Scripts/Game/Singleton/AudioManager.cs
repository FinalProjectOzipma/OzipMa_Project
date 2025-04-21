using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using static UnityEngine.Rendering.HDROutputUtils;

public class AudioManager
{

    [Range(0f, 1f)] public float masterVolume = 1f; // 마스터 볼륨
    [Range(0f, 1f)] public float bgmVolume = 1f; // BGM 볼륨
    [Range(0f, 1f)] public float sfxVolume = 1f; // SFX 볼륨

    public AudioControler audioControler;


    public void Initialize()
    {
        Managers.Resource.Instantiate("AudioSource", go =>
        {
            go.transform.SetParent(Managers.Instance.transform);
            audioControler = go.GetComponent<AudioControler>();
            audioControler.Initialize();
            audioControler.audioManager = this;
        });
    }




}
