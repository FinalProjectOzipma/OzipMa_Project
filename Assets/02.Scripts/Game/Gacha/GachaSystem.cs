using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    RankType rankType;
    public MyUnit GetRandomUnit()
    {
        int rand = Random.Range(0, 100);
        RankType selectedRank;

        if (rand < 70) selectedRank = RankType.Normal; //70%
        else selectedRank = RankType.Rare; //임시용
        //else if (rand < 90) selectedRank = RankType.Rare; //20%
        //else if (rand < 98) selectedRank = RankType.Epic; //8%
        //else selectedRank = RankType.Legend; //2%

        var result = Util.TableConverter<DefaultTable.Gacha>(Managers.Data.Datas[Enums.Sheet.Gacha]);
        var row = result[(int)selectedRank];
        int key = row.Key[Random.Range(0, row.Key.Count)];
        MyUnit unit = new();
        //스프라이트 어떻게 넣어주지..?
        //unit.Init(key, );
        return unit;
    }
    public Tower GetRandomTower()
    {
        int rand = Random.Range(0, 100);
        RankType selectedRank;

        if (rand < 70) selectedRank = RankType.Normal; //70%
        else selectedRank = RankType.Rare; //임시용
        //else if (rand < 90) selectedRank = RankType.Rare; //20%
        //else if (rand < 98) selectedRank = RankType.Epic; //8%
        //else selectedRank = RankType.Legend; //2%

        var result = Util.TableConverter<DefaultTable.Gacha>(Managers.Data.Datas[Enums.Sheet.Gacha]);
        var row = result[(int)selectedRank];
        int key = row.Key[Random.Range(0, row.Key.Count)];
        Tower tower = new();
        //스프라이트 어떻게 넣어주지..?
        //Sprite image = Managers.Resource.;
        //tower.Init(key, image);
        return tower;
    }
}
