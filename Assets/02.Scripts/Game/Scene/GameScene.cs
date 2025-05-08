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

        Managers.Resource.Instantiate("MainLevel3", map => 
        {
            CurrentMap = map; 
        });
        Managers.Resource.Instantiate("InventoryUI");
        Managers.Resource.Instantiate("MainUI");
        Managers.Resource.Instantiate("BuildingSystem", bs => 
        {
            BuildingSystem.Instance?.Init();
            Managers.Data.LoadGameData(() =>
            {
                // 로드 데이터 실패 시 실행되는 곳
                // 파이어베이스에 데이터가 없으면 디폴트 인벤토리로 세팅해줘야 함. 
                DefaultTowerAdd();
                DefaultUnitAdd();
            });
        });
        Managers.Resource.Instantiate("Ending_Panel");
    }

    public override void Update()
    {
        Managers.Wave.Update();
    }

    public override void Exit()
    {
        
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
