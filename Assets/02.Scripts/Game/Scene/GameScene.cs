using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

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
        bool isGroupLoadFinished = false;        
        GameObject loading = null;
        Managers.Resource.Instantiate("LoadScene", go =>
        {
            go.SetActive(true);
            loading = go;
            // 그룹로드 
            Managers.Resource.LoadResourceLocationAsync(LabelAsync, () =>
            {
                isGroupLoadFinished = true;
            });
        });

        while(loading == null)
        {
            yield return null;
        }

        // 1. 사용자 인증 ==============================================================
#if UNITY_EDITOR
#else
        yield return Managers.Auth.AnonymousLoginCoroutine();
#endif
        
        // 2. 그룹 로드 완료되면 넘어가기 ==============================================
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
        while(!Managers.Data.IsGameDataLoadFinished)
        {
            yield return null;
        }

        // 4. 로딩창 끄고 게임 시작 ====================================================
        if(loading != null) Managers.Resource.Destroy(loading);
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
        Managers.Resource.Instantiate("BuildingSystem", bs =>
        {
            BuildingSystem.Instance?.Init();
        });
        Managers.Resource.Instantiate("Ending_Panel");
    }

    private void DefaultTowerAdd()
    {
        List<DefaultTable.Tower> Towers = Util.TableConverter<DefaultTable.Tower>(Managers.Data.Datas[Enums.Sheet.Tower]);
        for (int i = 0; i < Towers.Count; i++)
        {
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
    }
}
