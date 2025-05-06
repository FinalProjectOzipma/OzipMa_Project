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
        Managers.Resource.Instantiate("BuildingSystem");
        Managers.Resource.Instantiate("Ending_Panel");
    }

    public override void Update()
    {
        Managers.Wave.Update();
    }

    public override void Exit()
    {
        
    }
}
