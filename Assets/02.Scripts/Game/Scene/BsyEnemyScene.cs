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

        MyUnit unit = new MyUnit();
        unit.Init(1, null);
        Managers.Player.Inventory.Add<MyUnit>(unit);
        Managers.Wave.StartWave(0);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
