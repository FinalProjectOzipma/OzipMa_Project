using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class SceneBase
{
    public Action InitAction;
    public Action SingletonAction;
    public AsyncOperationHandle<IList<IResourceLocation>> LabelAsync { get; set; }

    public virtual void Enter()
    {
        InitAction?.Invoke();
        SingletonAction?.Invoke();
    }

    public virtual void Exit()
    {
        InitAction = null;
    }
}
