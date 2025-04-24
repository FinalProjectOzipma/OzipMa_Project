using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YgmLoadingScene : GameScene
{
    public YgmLoadingScene()
    {
    }

    public override void Enter()
    {
        base.Enter();
        DefaultUnitAdd(); // 인벤 데이터 추가
        Managers.Wave.StartWave(0);
        InitAction?.Invoke();

    }

    public override void Exit()
    {
        base.Exit();
    }

    private void DefaultUnitAdd()
    {

        Managers.Resource.LoadAssetAsync<GameObject>("Zombie_Brain", (prefab) =>
        {
            MyUnit unit = new MyUnit();
            unit.Init(0, prefab.GetComponent<MyUnitController>().sprite);
            Managers.Player.Inventory.Add<MyUnit>(unit);
        });

        for (int i = 0; i < 3; i++)
        {
            int key = i;
            Managers.Resource.LoadAssetAsync<GameObject>("LaserTower", (prefab) =>
            {
                Tower unit = new Tower();
                unit.Init(key, prefab.GetComponent<TowerControlBase>().Preview);
                Managers.Player.Inventory.Add<Tower>(unit);
            });
        }
    }
}
