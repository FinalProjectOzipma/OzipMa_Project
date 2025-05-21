using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SceneBase
{
    public GameScene()
    {
        LabelAsync = nameof(GameScene);
    }

    public override void Enter()
    {
        base.Enter();

        Managers.StartCoroutine(LoadGameScene());
    }

    public override void Update()
    {
        Managers.Wave.Update();
    }

    public override void Exit()
    {

    }

    IEnumerator LoadGameScene()
    {
        // 0. 로딩 화면 보여주기 =======================================================
        GameObject loading = GameObject.Find("LoadScene");

        // 1. 사용자 인증 ==============================================================
#if UNITY_EDITOR
#else
        yield return Managers.Auth.AnonymousLoginCoroutine();
#endif

        // 2. 그룹 로드 완료되면 넘어가기 ==============================================
        bool isGroupLoadFinished = false;
        Managers.Resource.LoadResourceLocationAsync(LabelAsync, () =>
        {
            isGroupLoadFinished = true;
        });
        while (!isGroupLoadFinished)
        {
            yield return null;
        }

        // 3. 필요한 것들 생성 =========================================================
        InstantiateGameObjs();

        // 4. 정보 로드 ================================================================
        Managers.Data.LoadGameData(() =>
        {
            // 로드 실패 시 
            // 파이어베이스에 데이터가 없으면 디폴트 인벤토리로 세팅
            DefaultTowerAdd();
            DefaultUnitAdd();
        });
        while (!Managers.Data.IsGameDataLoadFinished)
        {
            yield return null;
        }

        // 5. 가챠 파베 준비 ===========================================================
        if(GachaSystem.Instance == null) new GachaSystem();
        while (!GachaSystem.Instance.IsReady)
        {
            yield return null;
        }

        // 6. 로딩창 끄고 게임 시작 ====================================================
        if (loading != null)
            Managers.Resource.Destroy(loading);
        Managers.Wave.GameStart();
    }

    private void InstantiateGameObjs()
    {
        // 전부 로드는 미리 되어있기 때문에 바로 생성됨
        Managers.Resource.Instantiate("MainLevel3", map =>
        {
            CurrentMap = map;
        });
        Managers.Resource.Instantiate("InventoryUI");
        Managers.Resource.Instantiate("MainUI");
        Managers.Resource.Instantiate("BuildingSystem");
        Managers.Resource.Instantiate("Ending_Panel");
        Managers.Audio.OnSceneLoaded();

    }

    private void DefaultTowerAdd()
    {
        List<DefaultTable.Tower> Towers = Util.TableConverter<DefaultTable.Tower>(Managers.Data.Datas[Enums.Sheet.Tower]);
        for (int i = 0; i < Towers.Count; i++)
        {
#if UNITY_EDITOR
#else
            if (Towers[i].Rank != RankType.Normal) continue;
#endif


            int key = i;
            Managers.Resource.LoadAssetAsync<GameObject>($"{Towers[key].Name}Tower", original =>
            {
                Tower tower = new Tower();
                tower.Init(key, original.GetComponent<TowerControlBase>().Preview);
                Managers.Player.Inventory.Add<Tower>(tower);
            });
        }
    }

    private void DefaultUnitAdd()
    {

        Managers.Resource.LoadAssetAsync<GameObject>("Zombie_Brain", (prefab) =>
        {
            MyUnit unit = new MyUnit();
            unit.Init(0, prefab.GetComponent<MyUnitController>().sprite);
            Managers.Player.Inventory.Add<MyUnit>(unit);
        });

#if UNITY_EDITOR
        Managers.Resource.LoadAssetAsync<GameObject>("Skeleton_Brain", (prefab) =>
        {
            MyUnit unit = new MyUnit();
            unit.Init(1, prefab.GetComponent<MyUnitController>().sprite);
            Managers.Player.Inventory.Add<MyUnit>(unit);
        });

        Managers.Resource.LoadAssetAsync<GameObject>("Vampire_Brain", (prefab) =>
        {
            MyUnit unit = new MyUnit();
            unit.Init(2, prefab.GetComponent<MyUnitController>().sprite);
            Managers.Player.Inventory.Add<MyUnit>(unit);
        });
#endif
    }
}
