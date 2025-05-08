using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public GameObject MainProps;
    public GameObject BossProps;

    private void Start()
    {
        Managers.Wave.OnStartBossMap += StartBossMap;
        Managers.Wave.OnEndBossMap += EndBossMap;
    }

    public void StartBossMap()
    {
        MainProps.SetActive(false);
        BossProps.SetActive(true);
    }
    public void EndBossMap()
    {
        MainProps.SetActive(true);
        BossProps.SetActive(false);
    }
}
