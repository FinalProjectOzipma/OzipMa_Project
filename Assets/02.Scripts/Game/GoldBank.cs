using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBank : MonoBehaviour
{
    public static GoldBank instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public int gold;
    public int zam;

    public event Action<int> OnGoldChanged;
    public event Action<int> OnZamChanged;

    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
    }

    public void AddZam(int amount)
    {
        zam += amount;
        OnGoldChanged?.Invoke(zam);
    }

    public void SpenGold(int amount)
    {
        if (gold < amount)
        {
            Debug.Log("돈이 부족합니다");
            //alarmPopupUI.gameObject.SetActive(true);
            //alarmPopupUI.ChangeAlarmText("돈이 부족합니다.");
            return;
        }

        gold -= amount;

        if (gold <= 0) gold = 0;

        OnGoldChanged?.Invoke(gold);
    }

    public void SpenZam(int amount)
    {
        if (zam < amount)
        {
            Debug.Log("돈이 부족합니다");
            //alarmPopupUI.gameObject.SetActive(true);
            //alarmPopupUI.ChangeAlarmText("돈이 부족합니다.");
            return;
        }

        zam -= amount;

        if (zam <= 0) gold = 0;

        OnGoldChanged?.Invoke(zam);
    }

    public int GetGold() => gold;
    public int GetZam() => zam;

}
