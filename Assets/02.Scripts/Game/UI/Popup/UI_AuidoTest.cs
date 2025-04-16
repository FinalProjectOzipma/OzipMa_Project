using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine;
using UnityEditor.Search;
using UnityEngine.AddressableAssets;

public class UI_AuidoTest : MonoBehaviour
{
    public GameObject audioPrefab;
    public List<AudioClip> bgmClip;

    private void Awake()
    {
    }
 
    private void Start()
    {
      Managers.Resource.LoadAssetAsync<GameObject>("AudioSource", go =>audioPrefab = go );

        Addressables.LoadAssetsAsync<AudioClip>("BGM", clip =>
        {
            bgmClip.Add(clip);
        });


    }

    private void Update()
    {

    }
}
