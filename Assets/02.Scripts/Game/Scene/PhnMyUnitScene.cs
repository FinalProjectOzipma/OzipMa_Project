using System.Collections.Generic;
using UnityEngine;

public class PhnMyUnitScene : GameScene
{
    public PhnMyUnitScene()
    {
    }

    public override void Enter()
    {
        base.Enter();


        // 테스트용
        //List<DefaultTable.Tower> Towers = Util.TableConverter<DefaultTable.Tower>(Managers.Data.Datas[Enums.Sheet.Tower]);
        //for (int i = 0; i < Towers.Count; i++)
        //{
        //    int key = i;
        //    Managers.Resource.LoadAssetAsync<GameObject>($"{Towers[key].Name}Tower", original =>
        //    {
        //        Tower tower = new Tower();
        //        tower.Init(key, original.GetComponent<TowerControlBase>().Preview);
        //        Managers.Player.Inventory.Add<Tower>(tower);
        //    });
        //}

        // 파이어베이스 테스트
        //Managers.Data.SaveGameData();
        //Managers.Data.LoadGameData(() => 
        //{
        //    // TODO :: 파이어베이스에 데이터가 없으면 디폴트 인벤토리로 세팅해줘야 함. 
        //    DefaultTowerAdd();
        //    DefaultUnitAdd(); // 인벤 데이터 추가
        //});

        // 인벤 데이터 추가
        //DefaultTowerAdd();
        //DefaultUnitAdd(); 
        InitAction?.Invoke();
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
        Managers.Resource.LoadAssetAsync<GameObject>("Reaper_Brain", (prefab) =>
        {
            MyUnit unit = new MyUnit();
            unit.Init(3, prefab.GetComponent<MyUnitController>().sprite);
        });
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
