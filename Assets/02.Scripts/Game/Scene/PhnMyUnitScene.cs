using System.Collections;
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

        InitAction?.Invoke();
        DefaultUnitAdd(); // 인벤 데이터 추가
        Managers.Wave.StartWave(0);
        InitAction?.Invoke();
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

    public override void Exit()
    {
        base.Exit();
    }
}
