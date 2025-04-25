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
        });
    }

    public void StartWave(int idx)
    {
        // 순서대로 처리해줘
        // TODO:: 알아서해 Feat: 박한나
        int needAmount = waveList[idx].EnemyAmount;
        Managers.StartCoroutine(Spawn(1));
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
        int random = UnityEngine.Random.Range(0, enemyList.Count);

        DefaultTable.Enemy spawnenemy = enemyList[random];

        /// 테스트 코드
        if(spawnenemy.IsBoss == 1)
        {
            spawnenemy = enemyList[0];
            random = 0;
        }
        
        string name = spawnenemy.Name;

        Managers.Resource.Instantiate($"{name}_Brain", (go) =>
        {
            CurEnemyList.Add(go);
            EnemyController ctrl = go.GetComponent<EnemyController>();
            ctrl.TakeRoot(random, name, enemySpawn.transform.position);
        });
    }

}
