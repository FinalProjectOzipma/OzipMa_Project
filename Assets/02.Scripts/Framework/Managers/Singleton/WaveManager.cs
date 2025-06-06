using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Table = DefaultTable;

public class WaveManager
{
    private float hubTime = 5f;
    public float Timer { get; set; }

    private PlayerManager playerManager;

    private Coroutine delayCoroutine;

    private List<Table.Wave> waveList;
    private List<Table.Enemy> enemyList;
    private List<Table.Enemy> bossList;

    public List<EnemyController> CurEnemyList;
    public List<MyUnitController> CurMyUnitList;
    public Dictionary<Vector3Int, Tower> CurTowerDict;

    public event Action OnStartBossMap;
    public event Action OnEndBossMap;

    private WaitForSeconds spawnTime = new WaitForSeconds(0.5f);
    private WaitForSeconds waveDelayTime = new WaitForSeconds(2f);
    private bool onSpawn = true;

    public GameObject enemySpawn;

    public CoreController MainCore { get; set; }
    public Enums.WaveState CurrentState { get; set; }
    public long CurrentGold { get; set; }
    public long CurrentGem { get; set; }

    public Queue<FieldReward> FieldRewards { get; set; } = new();

    public float PlayTime { get; set; }

    public void Initialize()
    {
        playerManager = Managers.Player;

        bossList = new();
        waveList = Util.TableConverter<Table.Wave>(Managers.Data.Datas[Enums.Sheet.Wave]);
        enemyList = Util.TableConverter<Table.Enemy>(Managers.Data.Datas[Enums.Sheet.Enemy]);
        CurEnemyList = new();
        CurMyUnitList = new();
        CurTowerDict = new();

        CurrentState = Enums.WaveState.None;
        Timer = 0;

        // 보스관련애들 넣어두기
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].IsBoss == 1)
                bossList.Add(enemyList[i]);
        }

        Managers.Resource.Instantiate("EnemySpawn", go =>
        {
            enemySpawn = go;
        });
    }

    public void GameStart()
    {
        CurrentState = Enums.WaveState.Start;
        Managers.Quest.UpdateQuestProgress(ConditionType.Connection, -1, 1);
    }

    bool isCoreDead = false;
    bool isEnemyAllDead = false;
    public void Update()
    {
        // 유저 데이터가 없으면 null처리
        if (playerManager == null)
            return;

        if (Timer >= 0.0f && CurrentState != Enums.WaveState.Playing)
            Timer -= Time.deltaTime;

        if (Timer <= 0.0f)
        {

            if (CurrentState == Enums.WaveState.Start)
            {
                if (onSpawn)
                    StartWave(playerManager.CurrentWave);

                onSpawn = false;
                // CurrentState는 비동기에서 처리
            }

            if (CurrentState == Enums.WaveState.Playing)
            {
                isCoreDead = (MainCore.core.Health.Value <= 0.0f);
                isEnemyAllDead = (CurEnemyList.Count == 0);


                if (isCoreDead || isEnemyAllDead)
                {
                    CurrentState = Enums.WaveState.Reward;
                    Managers.Effect.InvokeEffect<RewardEffect>(isEnemyAllDead);
                }
                else
                {
                    PlayTime += Time.deltaTime;
                }
                
            }

            if (CurrentState == Enums.WaveState.Reward)
                CurrentState = isCoreDead ? Enums.WaveState.End : Enums.WaveState.Reward;


            // End 변환점은 FieldGold 클래스에 있음 보상 다 받으면 실행
            if (CurrentState == Enums.WaveState.End)
            {
                foreach (var unit in CurMyUnitList)
                    unit.EntityDestroy();

                foreach (var enemy in CurEnemyList)
                    enemy.EntityDestroy();

                Managers.Resource.Destroy(MainCore.gameObject);

                CurEnemyList.Clear();
                CurMyUnitList.Clear();

                // 플레이어 측에서 이겼으면 웨이브 증가
                if (isEnemyAllDead)
                {
                    int clearWaveNumber = playerManager.CurrentWave;
                    var stages = Util.TableConverter<DefaultTable.Stage>(Managers.Data.Datas[Enums.Sheet.Stage]);
                    int EndNumber = stages[playerManager.CurrentKey].StageEnd;
                    Managers.Quest.UpdateQuestProgress(ConditionType.WaveClear, -1 , 1);


                    Managers.UI.GetScene<UI_EndingPanel>().MoveEndingPanel(true);

                    if (++playerManager.CurrentWave % 10 == 0)
                    {
                        Managers.Quest.UpdateQuestProgress(ConditionType.StageClear, -1, 1);
                        Managers.Quest.UpdateQuestProgress(ConditionType.BossKill, -1, 1);
                        if (++playerManager.CurrentStage > EndNumber)
                            playerManager.CurrentKey = Mathf.Min(++playerManager.CurrentKey, stages.Count - 1); // 스테이지 끝이면 현재 키를 늘린다.
                        playerManager.CurrentWave = 0;

                        #region 퍼널 다음 스테이지 ( 15 )
                        Managers.Analytics.SendFunnelStep(15);
                        #endregion
                    }

                    Managers.UI.GetScene<UI_EndingPanel>().SetRewardText(CurrentGold, CurrentGem);
                    playerManager.OnStageWave();

                    // 애널리틱스
                    #region wave_completed
                    string rewardType = (clearWaveNumber == 9) ? "Gem" : "Gold";
                    int rewardAmount = (clearWaveNumber == 9) ? (int)CurrentGem : (int)CurrentGold;
                    

                    Managers.Analytics.AnalyticsWaveCompleted(clearWaveNumber, CurMyUnitList.Count, rewardType, rewardAmount, PlayTime,
                        CurTowerDict.Values.ToArray(), CurMyUnitList.ToArray());
                    #endregion
                }
                else
                {
                    Managers.UI.GetScene<UI_EndingPanel>().MoveEndingPanel(false);

                    // 애널리틱스
                    #region wave_failed
                    Managers.Analytics.AnalyticsWaveFailed(playerManager.CurrentWave, CurEnemyList.Count, PlayTime, CurTowerDict.Count,
                        CurTowerDict.Values.ToArray(), CurMyUnitList.ToArray());
                    #endregion
                }

                Timer = hubTime;
                onSpawn = true;

                CurrentGold = 0;
                CurrentGem = 0;
                PlayTime = 0f;
                CurrentState = Enums.WaveState.Start;
            }
        }
    }

    public void StartWave(int idx)
    {
        Managers.Resource.Instantiate("Core_Brain", go =>
        {

            MainCore = go.GetComponent<CoreController>();
            MainCore.Init(Managers.Player.MainCoreData.Health.GetValue());
            int needAmount = waveList[idx].EnemyAmount;
            Managers.StartCoroutine(Spawn(needAmount));

            // 애널리틱스
            #region wave_started
            Managers.Analytics.AnalyticsWaveStarted(playerManager.CurrentWave, needAmount,
                CurTowerDict.Count, MainCore.core.Health.Value, (int)playerManager.Gold);
            #endregion

            // 애널리틱스 퍼널 로딩 씬 진입 시
            #region 웨이브 시작 시 (퍼널 3 ~ 12)
            Managers.Analytics.SendFunnelStep(playerManager.CurrentWave + 3);
            #endregion
        });
    }

    public IEnumerator Spawn(int amount)
    {
        bool isBoss = false;
        while (amount > 0)
        {
            yield return spawnTime;
            MainCore.SpawnUnit();

            if (!isBoss)
                isBoss = SpawnEnemy();

            amount--;
        }
        CurrentState = Enums.WaveState.Playing;
    }

    private bool SpawnEnemy()
    {
        int random = UnityEngine.Random.Range(0, enemyList.Count);

        DefaultTable.Enemy spawnenemy = enemyList[random];

        // CurrentWave : 0 ~ 9
        if (playerManager.CurrentWave == 9)
        {
            int index = playerManager.CurrentStage % bossList.Count;
            spawnenemy = bossList[index];
            random = bossList[index].EnemyPrimaryKey;

            OnStartBossMap?.Invoke();

            #region 퍼널 보스 등장 시 ( 13 )
            Managers.Analytics.SendFunnelStep(13);
            #endregion

        }
        else
        {
            if (spawnenemy.IsBoss == 1)
            {
                spawnenemy = enemyList[0];
                random = 0;
            }

            if (playerManager.CurrentWave == 0)
                OnEndBossMap?.Invoke();
        }


        string name = spawnenemy.Name;

        Managers.Resource.Instantiate($"{name}_Brain", (go) =>
        {
            EnemyController ctrl = go.GetComponent<EnemyController>();
            ctrl.TakeRoot(spawnenemy.EnemyPrimaryKey, name, (playerManager.CurrentWave == 9) ? new Vector2(enemySpawn.transform.position.x, -3.2f) : enemySpawn.transform.position);

            // 웨이브 몬스터 추가
            Managers.Wave.CurEnemyList.Add(ctrl);
        });

        return (playerManager.CurrentWave == 9); // 보스웨이브면 true
    }
}
