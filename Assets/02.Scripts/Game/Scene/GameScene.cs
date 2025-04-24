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

        Managers.Resource.Instantiate("MainLevel3", map => { CurrentMap = map; });
        Managers.Resource.Instantiate("InventoryUI");
        Managers.Resource.Instantiate("MainUI");

    }

    public override void Exit()
    {
        base.Exit();
    }
}
