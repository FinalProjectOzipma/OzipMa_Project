using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager
{
    public SceneBase CurrentScene { get; private set; }
    public GameScene GameScene { get; private set; } = new();
    public BsyEnemyScene BsyEnemyScene { get; private set; } = new();
    public PhnMyUnitScene PhnMyUnitScene { get; private set; } = new();
    public YgmLoadingScene YgmLoadingScene { get; private set; } = new();
    public PydTowerScene PydTowerScene { get; private set; } = new();
    public StartScene StartScene { get; private set; } = new();

    public void Initialize()
    {
        CurrentScene = StartScene;
    }

    public void ChangeScene<T>(T nextScene) where T : SceneBase
    {
        CurrentScene?.Exit();
        CurrentScene = nextScene;

        //UI_Loading.LoadScene(typeof(T).Name);
        UnityEngine.SceneManagement.SceneManager.LoadScene(typeof(T).Name);
        Managers.Resource.LoadResourceLocationAsync(nextScene.LabelAsync, nextScene.Enter);
    }
}
