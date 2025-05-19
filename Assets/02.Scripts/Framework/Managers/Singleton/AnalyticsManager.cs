using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    public static void SendFunnelStep(string stepNumber)
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
}