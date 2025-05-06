using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public abstract class SceneBase
{
    public Action InitAction;
    public Action disAction;
    public Action SingletonAction;
    public string LabelAsync { get; set; }
    public GameObject CurrentMap { get; set; }

    public virtual void Enter()
    {
        SingletonAction?.Invoke();
        InitAction?.Invoke();
    }

    public abstract void Update();
    public abstract void Exit();
}
