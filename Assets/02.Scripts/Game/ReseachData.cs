using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class ReseachData
{
    public string researchUpgradeType; // 연구 타입

    public string startTime; // 업그레이드 시작 시간

    public float researchDuration; // 연구 시간


    public int updateLevel; // 업데이트 레벨
    public float updateStat; // 업데이트 스탯

    public string spendGold; // 업그레이드 필요 골드
    public string spendZam; // 업그레드 필요 잼

    public string startKey; // 시작키 게임 종료 후 지난 시간 계산에 필요
    public string durationKey; // 경과 시간에 필요한 키
    public string levelKey; // 업그레이드 레벨 키
    public string updateStatKey; // 업데이트 스탯 저장 키
    public string spendGoldKey; // 업그레이드 필요 골드 키
    public string spendZamKey; // 업그레이드 필요 잼 키

}
