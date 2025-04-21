using GoogleSheet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Table = DefaultTable;

public class WaveManager
{
    private CoreBase mainCore;
    private Coroutine enemyCoroutine;
    private Coroutine unitCoroutine;
    private int liveEnemyCount = 0;

    private List<Table.Wave> waveList;
    private List<Table.Enemy> enemyList;
    private List<GameObject> curspawnEnemyList;

    private WaitForSeconds spawnTime = new WaitForSeconds(0.5f);

    private GameObject enemySpawn;

    public void Initialize()
    {
        waveList = Util.TableConverter<Table.Wave>(Managers.Data.Datas[Enums.Sheet.Wave]);
        enemyList = Util.TableConverter<Table.Enemy>(Managers.Data.Datas[Enums.Sheet.Enemy]);
        curspawnEnemyList = new();
        
        Managers.Resource.Instantiate("EnemySpawn", go =>
        {
            enemySpawn = go;
            SpawnEnemy();
        });

        Managers.Resource.Instantiate("Zombie_Brain", (go) =>
        {
            MyUnitController ctrl = go.GetComponent<MyUnitController>();
            ctrl.TakeRoot(1, "Zombie", Vector2.zero);
        });
    }

    public void StartWave(int id)
    {
        int needEnemyAmount = waveList[id].EnemyAmount;
        //int needMyUnitAmount = 5;

        liveEnemyCount = needEnemyAmount;
        // 코루틴 시작
        if (enemyCoroutine != null) Managers.MonoInstance.StopCoroutine(enemyCoroutine);
        if (unitCoroutine != null) Managers.MonoInstance.StopCoroutine(unitCoroutine);

        //enemyCoroutine = Managers.MonoInstance.StartCoroutine(EnemySpawnCoroutine(needEnemyAmount, waveTable.SpawnTime, enemyTable));
        //unitCoroutine = Managers.MonoInstance.StartCoroutine(MyUnitSpawnCoroutine(needMyUnitAmount, waveTable.SpawnTime));
    }

    private IEnumerator EnemySpawnCoroutine(int spawnAmount, List<GoogleSheet.ITable> enemyTable)
    {
        while (spawnAmount > 0)
        {
            spawnAmount--;
            int selected = UnityEngine.Random.Range(1, enemyTable.Count + 1);
            DefaultTable.Enemy selectedEnemyInfo = enemyTable[selected] as DefaultTable.Enemy;
            Managers.Resource.Instantiate(selectedEnemyInfo.Name);
            yield return spawnTime;
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
            yield return spawnTime;
        }
    }

    private void SpawnEnemy()
    {
        int random = UnityEngine.Random.Range(0, 3);

        DefaultTable.Enemy spawnenemy = enemyList[random];

        string name = spawnenemy.Name;

        Managers.Resource.Instantiate($"{name}_Brain", (go) =>
        {
            curspawnEnemyList.Add(go);
            EnemyController ctrl = go.GetComponent<EnemyController>();
            ctrl.Target = GameObject.Find("Test");
            ctrl.TakeRoot(random, $"{name}", enemySpawn.transform.position);    
        });

    }
}
