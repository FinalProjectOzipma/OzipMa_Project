using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager 
{
    public CoreBase MainCore { get; set; }
    public int Money { get; set; }
    public long gold { get; private set; }
    public long zam { get; private set; }

    public event Action<long> OnGoldChanged;
    public event Action<long> OnZamChanged;

    private string myGoldKey = "myGold";
    private string myZamKey = "myZam";
    public Inventory Inventory { get; set; } = new Inventory();

    public GameObject mainCore;

    public void Initialize()
    {
        // 처음 시작할때 선언
        Inventory = new Inventory();
        // 저장된게 있으면 선언
        // Inventory = 가져오는거

        Managers.Resource.Instantiate("Core", go => mainCore = go);

        gold = PlayerPrefs.HasKey(myGoldKey) ? long.Parse(PlayerPrefs.GetString(myGoldKey)) : 1000L;
        zam = PlayerPrefs.HasKey(myZamKey) ? long.Parse(PlayerPrefs.GetString(myZamKey)) : 100L;

        MainCore = new CoreBase();
    }

    /// <summary>
    /// 골드 추가
    /// </summary>
    public void AddGold(long amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }

    /// <summary>
    /// 잼 추가
    /// </summary>
    public void AddZam(long amount)
    {
        zam += amount;
        OnZamChanged?.Invoke(zam);
    }

    /// <summary>
    /// 골드 소모
    /// </summary>
    public void SpenGold(long amount)
    {
        if (gold < amount)
        {
            Debug.Log("돈이 부족합니다");
            return;
        }

        gold -= amount;

        if (gold <= 0) gold = 0;

        OnGoldChanged?.Invoke(gold);
    }

    /// <summary>
    /// 잼 소모
    /// </summary>
    public void SpenZam(long amount)
    {
        if (zam < amount)
        {
            Debug.Log("돈이 부족합니다");
            return;
        }

        zam -= amount;

        if (zam <= 0) gold = 0;

        OnZamChanged?.Invoke(zam);
    }

    /// <summary>
    /// 다른 곳에서 골드 사용하기
    /// </summary>
    public long GetGold() => gold;

    /// <summary>
    /// 다른 곳에서 잼 사용하기
    /// </summary>
    public long GetZam() => zam;

    public void SaveEcomomy()
    {
        PlayerPrefs.SetString(myGoldKey, gold.ToString());
        PlayerPrefs.SetString(myZamKey, zam.ToString());
    }


    public void SpawnUnit()
    {
        List<IGettable> myUnitsList = Managers.Player.Inventory.GetList<MyUnit>();

        int random = UnityEngine.Random.Range(0,3);

         MyUnit myUnit = myUnitsList[random].GetClassAddress<MyUnit>();

        string name = myUnit.Name;

        Managers.Resource.Instantiate($"{name}_Brain", (go) =>
        {
            MyUnitController ctrl = go.GetComponent<MyUnitController>();
            ctrl.Target = GameObject.Find("Test");
            ctrl.TakeRoot(random, $"{name}", mainCore.transform.position);
        });
    }
}
