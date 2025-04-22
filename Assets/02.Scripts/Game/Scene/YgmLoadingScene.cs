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
        Managers.Resource.Instantiate("MainUI");
        Managers.Resource.Instantiate("ReseachUI");
        Managers.Resource.Instantiate("SettingUI");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
