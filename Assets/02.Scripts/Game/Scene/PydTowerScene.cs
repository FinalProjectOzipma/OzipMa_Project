using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PydTowerScene : GameScene
{
    public PydTowerScene()
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
