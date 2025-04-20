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

    private WaitForSeconds spawnTime = new WaitForSeconds(0.5f);

    public void Initialize()
    {
        waveList = Util.TableConverter<Table.Wave>(Managers.Data.Datas[Enums.Sheet.Wave]);
        enemyList = Util.TableConverter<Table.Enemy>(Managers.Data.Datas[Enums.Sheet.Enemy]);


        Managers.Resource.Instantiate("SwordMan_Brain", (go) =>
        {
            EnemyController ctrl = go.GetComponent<EnemyController>();
            ctrl.Target = GameObject.Find("Test");
            ctrl.TakeRoot(0, "SwordMan", Vector2.zero);
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
}
