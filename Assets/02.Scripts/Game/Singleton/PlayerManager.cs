using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
///  유저가 저장해야될 필수적인 요소들을 저장하는 곳
/// </summary>
public class PlayerManager 
{
    public Core MainCoreData { get; set; }
    public long Gold { get; private set; }
    public long Gem { get; private set; }

    public event Action<long> OnGoldChanged;
    public event Action<long> OnZamChanged;
    public event Action<int, int> OnStageChanged;

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
        Gold += amount;
        OnGoldChanged?.Invoke(Gold);
    }

    /// <summary>
    /// 잼 추가
    /// </summary>
    public void AddGem(long amount)
    {
        Gem += amount;
        OnZamChanged?.Invoke(Gem);
    }

    /// <summary>
    /// 골드 소모
    /// </summary>
    public void SpenGold(long amount)
    {
        if (Gold < amount)
        {
            Debug.Log("돈이 부족합니다");
            return;
        }

        Gold -= amount;

        if (Gold <= 0) Gold = 0;

        OnGoldChanged?.Invoke(Gold);
    }

    /// <summary>
    /// 잼 소모
    /// </summary>
    public void SpenZam(long amount)
    {
        if (Gem < amount)
        {
            Debug.Log("돈이 부족합니다");
            return;
        }

        Gem -= amount;

        if (Gem <= 0) Gold = 0;

        OnZamChanged?.Invoke(Gem);
    }

    /// <summary>
    /// 다른 곳에서 골드 사용하기
    /// </summary>
    public long GetGold() => Gold;

    /// <summary>
    /// 다른 곳에서 잼 사용하기
    /// </summary>
    public long GetZam() => Gem;

    public void SaveEcomomy()
    {
        PlayerPrefs.SetString(myGoldKey, Gold.ToString());
        PlayerPrefs.SetString(myZamKey, Gem.ToString());
    }

    public void SetGoldAndJame()
    {
        Gold = PlayerPrefs.HasKey(myGoldKey) ? long.Parse(PlayerPrefs.GetString(myGoldKey)) : 100000L;
        Gem = PlayerPrefs.HasKey(myZamKey) ? long.Parse(PlayerPrefs.GetString(myZamKey)) : 1000L;
    }


    public void OnStageWave()
    {
        OnStageChanged?.Invoke(CurrentStage, CurrentWave);
    }

    public string GetStage() => CurrentStage.ToString();

}
