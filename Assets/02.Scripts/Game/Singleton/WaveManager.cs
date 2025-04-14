using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
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
        List<GoogleSheet.ITable> enemyTable = dataManager.Datas["EnemyTable"];
        int needEnemyAmount = waveTable.EnemyAmount;
        int needMyUnitAmount = 5;

        liveEnemyCount = needEnemyAmount;

        // 코루틴 시작
        if (enemyCoroutine != null) StopCoroutine(enemyCoroutine);
        if (unitCoroutine != null) StopCoroutine(unitCoroutine);

        enemyCoroutine = StartCoroutine(EnemySpawnCoroutine(needEnemyAmount, waveTable.SpawnTime, enemyTable));
        unitCoroutine = StartCoroutine(MyUnitSpawnCoroutine(needMyUnitAmount, waveTable.SpawnTime));
    }

    private IEnumerator EnemySpawnCoroutine(int spawnAmount, float spawnTime, List<GoogleSheet.ITable> enemyTable)
    {
        while (spawnAmount > 0)
        {
            spawnAmount--;
            int selected = UnityEngine.Random.Range(1, enemyTable.Count + 1);
            DefaultTable.Enemy selectedEnemyInfo = enemyTable[selected] as DefaultTable.Enemy;
            Managers.Resource.Instantiate(selectedEnemyInfo.Name);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private IEnumerator MyUnitSpawnCoroutine(int spawnAmount, float spawnTime)
    {
        while (spawnAmount > 0)
        {
            spawnAmount--;
            // TODO: 아군 유닛 소환 로직
            // 예: 인벤토리 기반 소환
            // MyUnit selectedUnit = Managers.Inventory.GetRandomMyUnit();
            // Managers.Resource.Instantiate(selectedUnit.Name);
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
