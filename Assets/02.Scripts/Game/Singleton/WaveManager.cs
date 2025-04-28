using GoogleSheet;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Table = DefaultTable;

public class WaveManager
{
    private float hubTime = 5f;
    private float timer;

    private PlayerManager playerManager;

    private Coroutine enemyCoroutine;
    private Coroutine unitCoroutine;

    private List<Table.Wave> waveList;
    private List<Table.Enemy> enemyList;
    private List<Table.Enemy> bossList;

    public List<GameObject> CurEnemyList;
    public List<GameObject> CurMyUnitList;

    private WaitForSeconds spawnTime = new WaitForSeconds(0.5f);
    private bool onSpawn = true;

    private GameObject enemySpawn;

    public CoreController MainCore { get; set; }
    public Enums.WaveState CurrentState { get; set; }

    public void Initialize()
    {
        playerManager = Managers.Player;

        bossList = new();
        waveList = Util.TableConverter<Table.Wave>(Managers.Data.Datas[Enums.Sheet.Wave]);
        enemyList = Util.TableConverter<Table.Enemy>(Managers.Data.Datas[Enums.Sheet.Enemy]);
        CurEnemyList = new();
        CurMyUnitList = new();

        CurrentState = Enums.WaveState.Start;
        timer = hubTime;

        // 보스관련애들 넣어두기
        for(int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].IsBoss == 1)
                bossList.Add(enemyList[i]);
        }

        Managers.Resource.Instantiate("EnemySpawn", go =>
        {
            enemySpawn = go;
        });
    }

    public void Update()
    {
        // 유저 데이터가 없으면 null처리
        if (playerManager == null)
            return;

        if (timer >= 0.0f && CurrentState == Enums.WaveState.Start)
            timer -= Time.deltaTime;

        if(timer <= 0.0f)
        {
            bool isCoreDead = false;
            bool isEnemyAllDead = false;

            if(CurrentState == Enums.WaveState.Start)
            {
                if(onSpawn)
                    StartWave(playerManager.CurrentWave);

                onSpawn = false;
                // CurrentState는 비동기에서 처리
            }
             
            if(CurrentState == Enums.WaveState.Playing)
            {
                isCoreDead = (MainCore.core.Health.Value <= 0.0f);
                isEnemyAllDead = (CurEnemyList.Count == 0);

                if (isCoreDead || isEnemyAllDead)
                    CurrentState = Enums.WaveState.End;
            }

            if(CurrentState == Enums.WaveState.End)
            {
                foreach(var unit in CurMyUnitList)
                    Managers.Resource.Destroy(unit);

                foreach(var enemy in CurEnemyList)
                    Managers.Resource.Destroy(enemy);

                Managers.Resource.Destroy(MainCore.gameObject);

                CurEnemyList.Clear();
                CurMyUnitList.Clear();

                // 플레이어 측에서 이겼으면 웨이브 증가
                if (isEnemyAllDead)
                {
                    if (++playerManager.CurrentWave % 10 == 0)
                    {
                        playerManager.CurrentStage++;
                        playerManager.CurrentWave = 0;
                    }
                }

                timer = hubTime;
                onSpawn = true;
                CurrentState = Enums.WaveState.Start;
            }
        }
    }

    public void StartWave(int idx)
    {
        // 순서대로 처리해줘
        // TODO:: 알아서해 Feat: 박한나
        Util.Log($"{playerManager.CurrentStage}");
        Util.Log($"{playerManager.CurrentWave}");
        Managers.Resource.Instantiate("Core", go => {

            MainCore = go.GetComponent<CoreController>();
            MainCore.Init(Managers.Player.MainCoreData.MaxHealth.Value);
            int needAmount = waveList[idx].EnemyAmount;
            Managers.StartCoroutine(Spawn(needAmount));
        });
    }

    public IEnumerator Spawn(int amount)
    {
        bool isBoss = false;
        while(amount > 0)
        {
            yield return spawnTime;
            MainCore.SpawnUnit();

            if(!isBoss)
                isBoss = SpawnEnemy();

            amount--;
        }

        CurrentState = Enums.WaveState.Playing;
    }

    private bool SpawnEnemy()
    {
        int random = UnityEngine.Random.Range(0, enemyList.Count);

        DefaultTable.Enemy spawnenemy = enemyList[random];

        // 테스트 코드
        if(playerManager.CurrentWave == 9)
        {
            int index = playerManager.CurrentStage % bossList.Count;
            spawnenemy = bossList[index];
            random = bossList[index].EnemyPrimaryKey;
        }
        else
        {
            if (spawnenemy.IsBoss == 1)
            {
                spawnenemy = enemyList[0];
                random = 0;
            }
        }
        
        
        string name = spawnenemy.Name;

        Managers.Resource.Instantiate($"{name}_Brain", (go) =>
        {
            EnemyController ctrl = go.GetComponent<EnemyController>(); 
            ctrl.TakeRoot(random, name, enemySpawn.transform.position);

            // 웨이브 몬스터 추가
            Managers.Wave.CurEnemyList.Add(ctrl.gameObject);
        });

        return (playerManager.CurrentWave == 9); // 보스웨이브면 true
    }

}
