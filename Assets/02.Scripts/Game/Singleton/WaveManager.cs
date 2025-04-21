using GoogleSheet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Table = DefaultTable;

public class WaveManager
{
    private Core mainCore;
    private Coroutine enemyCoroutine;
    private Coroutine unitCoroutine;

    private List<Table.Wave> waveList;
    private List<Table.Enemy> enemyList;

    public List<GameObject> CurEnemyList;
    public List<GameObject> CurMyUnitList;

    private WaitForSeconds spawnTime = new WaitForSeconds(0.5f);

    private GameObject enemySpawn;

    public void Initialize()
    {
        waveList = Util.TableConverter<Table.Wave>(Managers.Data.Datas[Enums.Sheet.Wave]);
        enemyList = Util.TableConverter<Table.Enemy>(Managers.Data.Datas[Enums.Sheet.Enemy]);
        CurEnemyList = new();
        CurMyUnitList = new();

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
        int needAmount = waveList[id].EnemyAmount;
        Managers.StartCoroutine(Spawn(needAmount));
        //enemyCoroutine = Managers.MonoInstance.StartCoroutine(EnemySpawnCoroutine(needEnemyAmount, waveTable.SpawnTime, enemyTable));
        //unitCoroutine = Managers.MonoInstance.StartCoroutine(MyUnitSpawnCoroutine(needMyUnitAmount, waveTable.SpawnTime));
    }

    public IEnumerator Spawn(int amount)
    {
        CoreController coreCtrl = Managers.Player.MainCore;

        while(amount > 0)
        {
            yield return spawnTime;
            coreCtrl.SpawnUnit();
            SpawnEnemy();
            amount--;
        }
    }

    private void SpawnEnemy()
    {
        int random = UnityEngine.Random.Range(0, 3);

        DefaultTable.Enemy spawnenemy = enemyList[random];

        string name = spawnenemy.Name;

        Managers.Resource.Instantiate($"{name}_Brain", (go) =>
        {
            CurEnemyList.Add(go);
            EnemyController ctrl = go.GetComponent<EnemyController>();
            ctrl.TakeRoot(random, $"{name}", enemySpawn.transform.position);
        });
    }
}
