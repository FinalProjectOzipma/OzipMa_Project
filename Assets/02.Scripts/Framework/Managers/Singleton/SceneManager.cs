using UnityEngine;
using DG.Tweening;

public class SceneManager
{
    public SceneBase CurrentScene { get; private set; }
    public GameScene GameScene { get; private set; } = new();
    public PhnMyUnitScene PhnMyUnitScene { get; private set; } = new();
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
        //UnityEngine.SceneManagement.SceneManager.LoadScene(typeof(T).Name);

        Managers.Resource.Instantiate("LoadScene", go =>
        {
            go.SetActive(true);
            
            Managers.Resource.LoadResourceLocationAsync(nextScene.LabelAsync, () =>
            {
                nextScene.Enter();

                DOVirtual.DelayedCall(0.0f, () =>
                {
                    Managers.Resource.Destroy(go);
                });
            });
        });
    }

}
