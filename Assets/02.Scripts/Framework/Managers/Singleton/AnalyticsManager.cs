using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultTable;
using System;

public class AnalyticsManager
{

    public async void Initialize()
    {
        try
        {
            // Unity Services 초기화
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();

            Debug.Log("Unity Services Initialized");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Unity Services failed to initialize: " + e.Message);
        }
    }
    //[1]퍼널
    public void SendFunnelStep(int stepNumber)
    {
        var funnelEvent = new CustomEvent("Funnel_Step"); //event
        funnelEvent["Funnel_Step_Number"] = stepNumber; //parameter

        AnalyticsService.Instance.RecordEvent(funnelEvent); //custom event
    }

    //[2]스킬
    public void AnalyticsUnitSummoned(string unit_id, string unit_name, string unit_type, int unit_level, string summon_source, int wave_number)
    {
        var UnitSummoned = new CustomEvent("unit_summoned"); //event
        UnitSummoned["unit_id"] = unit_id; //parameter
        UnitSummoned["unit_name"] = unit_name; //parameter
        UnitSummoned["unit_type"] = unit_type;
        UnitSummoned["unit_level"] = unit_level;
        UnitSummoned["summon_source"] = summon_source;
        UnitSummoned["wave_number"] = wave_number;


        AnalyticsService.Instance.RecordEvent(UnitSummoned); //custom event
    }

    public void AnalyticsTowerInstalled(string tower_id, string tower_type, int tower_level, int position_x, int position_y, int wave_number)
    {
        var TowerInstalled = new CustomEvent("tower_installed"); //event
        TowerInstalled["tower_id"] = tower_id; //parameter
        TowerInstalled["tower_type"] = tower_type; //parameter
        TowerInstalled["tower_level"] = tower_level;
        TowerInstalled["position_x"] = position_x;
        TowerInstalled["position_y"] = position_y;
        TowerInstalled["wave_number"] = wave_number;

        AnalyticsService.Instance.RecordEvent(TowerInstalled); //custom event
    }

    public void AnalyticsWaveStarted(int wave_number, int unit_count, int tower_count, float player_hp, int remaining_currency)
    {
        var WaveStarted = new CustomEvent("wave_started"); //event
        WaveStarted["wave_number"] = wave_number; //parameter
        WaveStarted["unit_count"] = unit_count; //parameter
        WaveStarted["tower_count"] = tower_count;
        WaveStarted["player_hp"] = player_hp;
        WaveStarted["remaining_currency"] = remaining_currency;

        AnalyticsService.Instance.RecordEvent(WaveStarted); //custom event
    }

    public void AnalyticsWaveCompleted(int wave_number, int remaining_units, string reward_type, int reward_amount, float total_play_time,
        Tower[] towers, MyUnitController[] units)
    {
        var WaveCompleted = new CustomEvent("wave_completed"); //event
        WaveCompleted["wave_number"] = wave_number; //parameter
        WaveCompleted["remaining_enemies"] = remaining_units; // 이녀석은 아군으로 해야될듯
        WaveCompleted["reward_type"] = reward_type;
        WaveCompleted["reward_amount"] = reward_amount;
        WaveCompleted["total_play_time"] = total_play_time;

        for (int i = 0; i < towers.Length; i++)
        {
            WaveCompleted[$"tower_{i + 1}"] = "null";
            if (towers[i] != null)
                WaveCompleted[$"tower_{i + 1}"] = towers[i].Name;
        }

        for (int i = 0; i < units.Length; i++)
        {
            WaveCompleted[$"unit_{i + 1}"] = "null";
            if (units[i] != null)
                WaveCompleted[$"unit_{i + 1}"] = units[i].MyUnit.Name;
        }

        AnalyticsService.Instance.RecordEvent(WaveCompleted); //custom event
    }

    public void AnalyticsWaveFailed(int wave_number, int remaining_enemies, float lasted_seconds, int tower_count,
        Tower[] towers, MyUnitController[] units)
    {
        var WaveFailed = new CustomEvent("wave_failed"); //event
        WaveFailed["wave_number"] = wave_number; //parameter
        WaveFailed["remaining_enemies"] = remaining_enemies;
        WaveFailed["lasted_seconds"] = lasted_seconds;
        WaveFailed["tower_count"] = tower_count;

        for (int i = 0; i < towers.Length; i++)
        {
            WaveFailed[$"tower_{i + 1}"] = "null";
            if (towers[i] != null)
                WaveFailed[$"tower_{i + 1}"] = towers[i].Name;
        }

        for (int i = 0; i < units.Length; i++)
        {
            WaveFailed[$"unit_{i + 1}"] = "null";
            if (units[i] != null)
                WaveFailed[$"unit_{i + 1}"] = units[i].MyUnit.Name;
        }

        AnalyticsService.Instance.RecordEvent(WaveFailed); //custom event
    }

    public void AnalyticsGachaUsed(string gacha_type, string currency_used, int currency_amount, int pull_count,
        string result_unit_ids, string rarity_distribution)
    {
        var GachaUsed = new CustomEvent("gacha_used"); //event
        GachaUsed["gacha_type"] = gacha_type; //parameter
        GachaUsed["currency_used"] = currency_used;
        GachaUsed["currency_amount"] = currency_amount;
        GachaUsed["pull_count"] = pull_count;
        GachaUsed["result_unit_ids"] = result_unit_ids;
        GachaUsed["rarity_distribution"] = rarity_distribution;

        AnalyticsService.Instance.RecordEvent(GachaUsed); //custom event
    }

    public void AnalyticsResearchStarted(string research_id, string research_name, int research_level, int wave_number, float research_duration)
    {
        var ResearchStarted = new CustomEvent("research_started"); //event
        ResearchStarted["research_id"] = research_id; //parameter
        ResearchStarted["research_name"] = research_name;
        ResearchStarted["research_level"] = research_level;
        ResearchStarted["wave_number"] = wave_number;
        ResearchStarted["research_duration"] = research_duration;

        AnalyticsService.Instance.RecordEvent(ResearchStarted); //custom event
    }

    public void AnalyticsResearchCompleted(string research_id, int research_level, float completion_time, 
        string resource_type, string resource_amount)
    {
        var ResearchCompleted = new CustomEvent("research_completed"); //event
        ResearchCompleted["research_id"] = research_id; //parameter
        ResearchCompleted["research_level"] = research_level;
        ResearchCompleted["completion_time"] = completion_time;
        ResearchCompleted["resource_type"] = resource_type;
        ResearchCompleted["Resource_amount"] = resource_amount;

        AnalyticsService.Instance.RecordEvent(ResearchCompleted); //custom event
    }

    public void AnalyticsDailyLogin(string player_id, string last_login_time, string current_login_time,
        float hours_since_last, int consecutive_days)
    {
        var DailyLogin = new CustomEvent("daily_login"); //event
        DailyLogin["player_id"] = player_id; //parameter
        DailyLogin["last_login_time"] = last_login_time;
        DailyLogin["current_login_time"] = current_login_time;
        DailyLogin["hours_since_last"] = hours_since_last;
        DailyLogin["consecutive_days"] = consecutive_days;

        AnalyticsService.Instance.RecordEvent(DailyLogin); //custom event
    }
}