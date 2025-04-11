using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class WaveManager
{
    private DataManager dataManager;
    private Coroutine enemyCoroutine;
    private Coroutine unitCoroutine;
    private int liveEnemyCount = 0;
    public void Initialize()
    {
        dataManager = Managers.Data;
    }

    public void StartWave(int id)
    {
        DefaultTable.Wave waveTable = dataManager.Datas["Wave"][id] as DefaultTable.Wave;
        List<object> enemyTable = dataManager.Datas["EnemyTable"]; // 에너미 테이블
        int needEnemyAmount = waveTable.EnemyAmount;

        liveEnemyCount = needEnemyAmount;
        EnemySpawn(needEnemyAmount, waveTable.SpawnTime, enemyTable);

        // TODO : 아군 웨이브 스폰 
        //MyUnitSpawn();
    }

    private void EnemySpawn(int spawnAmount, float spawnTime, List<object> enemyTable)
    {
        //if(enemyCoroutine != null)
        //{
        //    StopCoroutine(enemyCoroutine);
        //}
        //enemyCoroutine = StartCoroutine(spawnAmount, spawnTime, enemyTable);
    }

    private IEnumerator EnemySpawnCoroutine(int spawnAmount, float spawnTime, List<object> enemyTable)
    {
        while (spawnAmount > 0)
        {
            spawnAmount--;
            int selected = UnityEngine.Random.Range(1, enemyTable.Count + 1); // 1번부터 totalEnemyCount 중에 소환할 번호 뽑기
            DefaultTable.Enemy selectedEnemyInfo = enemyTable[selected] as DefaultTable.Enemy;
            Managers.Resource.Instantiate(selectedEnemyInfo.Name);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void MyUnitSpawn()
    {
        // TODO : 아군 몬스터 5마리 랜덤 소환
    }
}
