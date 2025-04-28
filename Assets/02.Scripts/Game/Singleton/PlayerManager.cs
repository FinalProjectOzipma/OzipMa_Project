using System;
using UnityEngine;

/// <summary>
///  유저가 저장해야될 필수적인 요소들을 저장하는 곳
/// </summary>
public class PlayerManager 
{
    public Core MainCoreData { get; set; }
    public int Money { get; set; }
    public long gold { get; private set; }
    public long zam { get; private set; }

    public event Action<long> OnGoldChanged;
    public event Action<long> OnZamChanged;

    private string myGoldKey = "myGold";
    private string myZamKey = "myZam";
    public Inventory Inventory { get; set; } = new Inventory();

    public int CurrentStage { get; set; }
    public int CurrentWave { get; set; }

    public void Initialize()
    {
        // 처음 시작할때 선언
        Inventory = new Inventory();
        MainCoreData = new Core();

        // 저장된게 있으면 선언
        // Inventory = 가져오는거

        SetGoldAndJame();
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

    public void SetGoldAndJame()
    {
        gold = PlayerPrefs.HasKey(myGoldKey) ? long.Parse(PlayerPrefs.GetString(myGoldKey)) : 1000000L;
        zam = PlayerPrefs.HasKey(myZamKey) ? long.Parse(PlayerPrefs.GetString(myZamKey)) : 100L;
    }

}
