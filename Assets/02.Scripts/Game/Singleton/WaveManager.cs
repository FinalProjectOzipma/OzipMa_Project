using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class WaveManager
{
    private DataManager dataManager;
    private int liveEnemyCount = 0;

    private Task EnemySpawnTask;
    private Task MyUnitSpawnTask;

    public void Initialize()
    {
        dataManager = Managers.Data;
    }

    //연두님 해줘 상윤님 해줘 근무님 해줘.. 비동기로 이걸 둬야함??? 아니면 그냥 보이드?
    public void StartWave(int id)
    {
        DefaultTable.Wave waveTable = dataManager.Datas["Wave"][id] as DefaultTable.Wave;
        
        List<object> enemyTable = dataManager.Datas["EnemyTable"]; // 에너미 테이블

        int needEnemyAmount = waveTable.EnemyAmount;
        int needMyUnitAmount = 5;

        liveEnemyCount = needEnemyAmount;

        Task EnemySpawnTask = EnemySpawn(needEnemyAmount, waveTable.SpawnTime, enemyTable);
        // TODO : 아군 웨이브 스폰 
        Task MyUnitSpawnTask = MyUnitSpawn(needMyUnitAmount, waveTable.SpawnTime);
    }

    private async Task EnemySpawn(int spawnAmount, float spawnTime, List<object> enemyTable)
    {
        while (spawnAmount > 0)
        {
            spawnAmount--;
            int selected = UnityEngine.Random.Range(1, enemyTable.Count + 1); // 1번부터 totalEnemyCount 중에 소환할 번호 뽑기
            DefaultTable.Enemy selectedEnemyInfo = enemyTable[selected] as DefaultTable.Enemy;
            Managers.Resource.Instantiate(selectedEnemyInfo.Name);
            await Task.Delay((int)(spawnTime * 1000));
        }
    }

    private async Task MyUnitSpawn(int spawnAmount, float spawnTime)
    {
        // TODO : 아군 몬스터 5마리 랜덤 소환
        while (spawnAmount > 0)
        {
            spawnAmount--;
            //int selected = UnityEngine.Random.Range(1, unitTable.Count + 1); // 1번부터 totalEnemyCount 중에 소환할 번호 뽑기
            //아래부분 인벤토리의 마이유닛으로 변환해야하는데 이거 해주세요...
            //DefaultTable.Enemy selectedMyUnitInfo = unitTable[selected] as DefaultTable.Enemy;
            //Managers.Resource.Instantiate(selectedMyUnitInfo.Name);
            await Task.Delay((int)(spawnTime * 1000));
        }
    }
}