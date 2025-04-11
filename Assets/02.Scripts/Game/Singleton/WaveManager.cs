using System;
using System.Collections.Generic;
using System.Threading.Tasks; 
using UnityEngine;

public class WaveManager
{
    private DataManager dataManager;
    private int liveEnemyCount = 0;

    public void Initialize()
    {
        dataManager = Managers.Data;
    }

    public async void StartWave(int id)
    {
        DefaultTable.Wave waveTable = dataManager.Datas["Wave"][id] as DefaultTable.Wave;
        List<object> enemyTable = dataManager.Datas["EnemyTable"];
        int needEnemyAmount = waveTable.EnemyAmount;

        liveEnemyCount = needEnemyAmount;

        await EnemySpawnAsync(needEnemyAmount, waveTable.SpawnTime, enemyTable); // [3] 코루틴이 아닌 비동기 함수 호출로 변경

        // TODO : 아군 웨이브 스폰
        //await MyUnitSpawnAsync(); // [6] 아군 스폰도 비동기로 확장 예정
    }

    private async Task EnemySpawnAsync(int spawnAmount, float spawnTime, List<object> enemyTable)
    {
        while (spawnAmount > 0)
        {
            spawnAmount--;
            int selected = UnityEngine.Random.Range(0, enemyTable.Count);
            DefaultTable.Enemy selectedEnemyInfo = enemyTable[selected] as DefaultTable.Enemy;

            Managers.Resource.Instantiate(selectedEnemyInfo.Name);

            await Task.Delay((int)(spawnTime * 1000));
        }
    }

    //// [7] 아군 스폰도 비동기로 확장 준비
    //private async Task MyUnitSpawnAsync()
    //{

    //}
}
