using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class SceneBase
{
    public Action InitAction;
    public Action SingletonAction;
    public string LabelAsync { get; set; }
    public GameObject CurrentMap { get; set; }

    public virtual void Enter()
    {
        SingletonAction?.Invoke();
        InitAction?.Invoke();
    }

    public virtual void Exit()
    {
        InitAction = null;
        // Managers.Resource.Release(LabelAsync); 각각의 씬마다 구현
    }
}
