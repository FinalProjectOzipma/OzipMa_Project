using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager
{

    public long gold { get; private set; }
    public long zam { get; private set; }

    public event Action<long> OnGoldChanged;
    public event Action<long> OnZamChanged;

    private string myGoldKey = "myGold";
    private string myZamKey = "myZam";

    public void Initialize()
    {
        gold = PlayerPrefs.HasKey(myGoldKey) ? long.Parse(PlayerPrefs.GetString(myGoldKey)) : 1000L;
        zam = PlayerPrefs.HasKey(myZamKey) ? long.Parse(PlayerPrefs.GetString(myZamKey)) : 100L;
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
        PlayerPrefs.SetString(myGoldKey,gold.ToString());
        PlayerPrefs.SetString(myZamKey, zam.ToString());
    }

    /// <summary>
    /// long => String
    /// </summary>
    public static string FormatNumber(long number)
    {
        if (number >= 1_000_000_000)
            return (number / 1_000_000_000f).ToString("0.0") + "B";
        else if (number >= 1_000_000)
            return (number / 1_000_000f).ToString("0.0") + "M";
        else if (number >= 1_000)
            return (number / 1_000f).ToString("0.0") + "K";
        else
            return number.ToString();
    }

}
