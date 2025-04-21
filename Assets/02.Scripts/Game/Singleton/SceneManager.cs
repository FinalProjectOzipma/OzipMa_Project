using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager
{
    public SceneBase CurrentScene { get; private set; }
    public GameScene GameScene { get; private set; } = new();

    public void Initialize()
    {
        CurrentScene = GameScene;
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
