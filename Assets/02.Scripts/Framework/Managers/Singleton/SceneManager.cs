using UnityEngine;
using DG.Tweening;
using System;

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

        nextScene.Enter();
    }

}
