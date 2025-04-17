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
        Managers.Resource.Instantiate("Audio", go =>
        {
            go.transform.SetParent(Managers.Instance.transform);
            audioControler = go.GetComponent<AudioControler>();
            audioControler.Initialize();
            audioControler.audioManager = this;
        });

        //Addressables.LoadResourceLocationsAsync("Audio").Completed += handle =>
        //{
        //    if (handle.Status != AsyncOperationStatus.Succeeded)
        //    {
        //        Debug.LogError("Audio 리소스 로딩 실패");
        //        return;
        //    }

        //    foreach (var location in handle.Result)
        //    {
        //        var key = location.PrimaryKey;

        //        if (key.Contains("AudioControler"))
        //        {
        //            Addressables.LoadAssetAsync<GameObject>(location).Completed += assetHandle =>
        //            {
        //                Debug.Log("오디오컨트롤러");
        //                GameObject go = GameObject.Instantiate(assetHandle.Result);
        //                audioControler = go.GetComponent<AudioControler>();
        //                go.transform.SetParent(Managers.Instance.transform);
        //                audioControler.Initialize();
        //                audioControler.audioManager = this;
        //            };
        //        }
        //    }
        //};
    }




}
