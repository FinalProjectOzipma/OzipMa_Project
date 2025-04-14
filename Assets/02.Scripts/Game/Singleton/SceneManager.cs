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
        CurrentScene.Enter();
    }

    public void ChangeScene<T>(SceneBase nextScene) where T : SceneBase
    {
        CurrentScene?.Exit();
        CurrentScene = nextScene;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameof(T));
        Managers.Resource.CreateGenericGroupOperation(nextScene.LabelAsync, CurrentScene.Enter);
    }
}
