using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GachaSystem
{
    //단일 유닛뽑기
    public MyUnit GetRandomUnit()
    {
        int rand = Random.Range(0, 100);
        RankType selectedRank;

        //랭크뽑기
        if (rand < 70) selectedRank = RankType.Normal; //70%
        else selectedRank = RankType.Rare; //TODO: 임시로 쓰는것. 랭크 추가시 지울것
        //else if (rand < 90) selectedRank = RankType.Rare; //20%
        //else if (rand < 98) selectedRank = RankType.Epic; //8%
        //else selectedRank = RankType.Legend; //2%

        return GetSelectUnit(selectedRank);
    }

    //단일 타워뽑기
    public Tower GetRandomTower()
    {
        int rand = Random.Range(0, 100);
        RankType selectedRank;

        //랭크뽑기
        if (rand < 70) selectedRank = RankType.Normal; //70%
        else if (rand < 90) selectedRank = RankType.Rare; //20%
        else selectedRank = RankType.Epic; //TODO: 임시로 쓰는것. 랭크 추가시 지울것
        //else if (rand < 98) selectedRank = RankType.Epic; //8%
        //else selectedRank = RankType.Legend; //2%

        return GetSelectTower(selectedRank);
    }

    //특정 랭크 유닛 뽑기
    public MyUnit GetSelectUnit(RankType rank)
    {
        //해당 랭크 중에서 유닛id뽑기
        var result = Util.TableConverter<DefaultTable.Gacha>(Managers.Data.Datas[Enums.Sheet.Gacha]);
        var row = result[(int)rank];
        int key = row.Key[Random.Range(0, row.Key.Count)]; //primarykey가져오기

        //유닛 데이터 로드해서 뽑기
        var unitData = Util.TableConverter<DefaultTable.MyUnit>(Managers.Data.Datas[Enums.Sheet.MyUnit]);
        string name = unitData[key].Name;

        MyUnit returnValue = new();

        Managers.Resource.LoadAssetAsync<GameObject>($"{name}_Brain", (prefab) =>
        {
            MyUnit unit = new MyUnit();
            unit.Init(key, prefab.GetComponent<MyUnitController>().sprite);
            returnValue = unit;
        });

        return returnValue;
    }

    public Tower GetSelectTower(RankType rank)
    {
        //해당 랭크 중에서 유닛id뽑기
        var result = Util.TableConverter<DefaultTable.Gacha>(Managers.Data.Datas[Enums.Sheet.Gacha]);
        //타워는 4칸 아래에 데이터가 있으니까
        var row = result[(int)rank + 4];
        int key = row.Key[Random.Range(0, row.Key.Count)]; //primarykey가져오기

        //타워 데이터 로드해서 뽑기
        var towerData = Util.TableConverter<DefaultTable.Tower>(Managers.Data.Datas[Enums.Sheet.Tower]);
        string name = towerData[key].Name;

        Tower returnValue = new();

        Managers.Resource.LoadAssetAsync<GameObject>($"{name}Tower", (prefab) =>
        {
            Tower tower = new Tower();
            tower.Init(key, prefab.GetComponent<TowerControlBase>().Preview);
            returnValue = tower;
        });

        return returnValue;
    }
}