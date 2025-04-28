using GoogleSheet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Table = DefaultTable;

public class WaveManager
{
    private Coroutine enemyCoroutine;
    private Coroutine unitCoroutine;

    public CoreController MainCore { get; set; }
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

        Managers.Resource.Instantiate("Core", go => {

            MainCore = go.GetComponent<CoreController>();
            go.GetComponent<CoreController>().Init(Managers.Player.MainCoreData);
            int needAmount = waveList[idx].EnemyAmount;
            Managers.StartCoroutine(Spawn(needAmount));
        });
    }

    public IEnumerator Spawn(int amount)
    {
        while(amount > 0)
        {
            yield return spawnTime;
            MainCore.SpawnUnit();
            //SpawnEnemy();
            amount--;
        }
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        int random = 1;//UnityEngine.Random.Range(1, enemyList.Count);

        DefaultTable.Enemy spawnenemy = enemyList[random];

        /// 테스트 코드
        /*if(spawnenemy.IsBoss == 1)
        {
            spawnenemy = enemyList[0];
        }*/
        
        string name = spawnenemy.Name;

        Managers.Resource.Instantiate($"{name}_Brain", (go) =>
        {
            CurEnemyList.Add(go);
            EnemyController ctrl = go.GetComponent<EnemyController>();
            ctrl.TakeRoot(random, name, enemySpawn.transform.position);
        });
    }

}
