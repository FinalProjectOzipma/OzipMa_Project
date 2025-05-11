using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    RankType rankType;
    public MyUnit GetRandomUnit(List<MyUnit> allUnits)
    {
        int rand = Random.Range(0, 100);
        RankType selectedRank;

        if (rand < 20) selectedRank = RankType.Normal; //20%
        else if (rand < 40) selectedRank = RankType.Rare; //20%
        else if (rand < 60) selectedRank = RankType.Epic; //20%
        else if (rand < 80) selectedRank = RankType.Myth; //20%
        else if (rand < 95) selectedRank = RankType.Legend; //15%
        else selectedRank = RankType.Ancient; // 5%

        var candidates = allUnits.Where(unit => unit.RankType == selectedRank).ToList();
        if (candidates.Count == 0) return null;

        return candidates[Random.Range(0, candidates.Count)];
    }
}
