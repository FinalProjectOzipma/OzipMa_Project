using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BsyEnemyScene : GameScene
{
    public BsyEnemyScene()
    {
    }

    public override void Enter()
    {
        base.Enter();

        Managers.Resource.LoadAssetAsync<GameObject>("Zombie_Brain", (prefab) =>
        {
            MyUnit unit = new MyUnit();
            unit.Init(1, prefab.GetComponent<MyUnitController>().sprite);
            Managers.Player.Inventory.Add<MyUnit>(unit);
        });

        Managers.Resource.LoadAssetAsync<GameObject>("Laser_Brain", (prefab) =>
        {
            Tower unit = new Tower();
            unit.Init(0, prefab.GetComponent<TowerControlBase>().Preview);
            Managers.Player.Inventory.Add<Tower>(unit);
        });

        Managers.Wave.StartWave(0);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
