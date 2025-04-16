using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using static UnityEngine.Rendering.HDROutputUtils;
using Object = UnityEngine.Object;

public class ResourceManager
{
    private readonly Dictionary<string, List<string>> keys = new();
    private readonly Dictionary<string, AsyncOperationHandle> operations = new();

    public AsyncOperationHandle LoadAssetAsync<T>(string key, Action<T> onComplete = null) where T : Object
    {
        if (operations.TryGetValue(key, out var operation))
        {
            onComplete?.Invoke(operation.Result as T);
            return operation;
        }

        operation = Addressables.LoadAssetAsync<T>(key);
        operation.Completed += operation => onComplete?.Invoke(operation.Result as T);
        operations.Add(key, operation);
        return operation;
    }

    private AsyncOperationHandle CreateGenericGroupOperation(AsyncOperationHandle<IList<IResourceLocation>> groupLocation, Action onComplete = null)
    {
        var locations = groupLocation.Result;
        var keys = new List<string>(locations.Count);
        var operations = new List<AsyncOperationHandle>(locations.Count);
        foreach (var location in locations)
        {
            string key = location.PrimaryKey;
            keys.Add(key);

            var operation = LoadAssetAsync<Object>(key);
            operations.Add(operation);
        }

        string label = groupLocation.DebugName;
        if (!this.keys.ContainsKey(label))
            this.keys.Add(label, keys);

        var groupOperation = Addressables.ResourceManager.CreateGenericGroupOperation(operations);
        groupOperation.Completed += operation => onComplete?.Invoke();

        if(!this.operations.ContainsKey(label))
            this.operations.Add(label, groupOperation);

        Addressables.Release(groupLocation);
        return groupOperation;
    }

    public void Release(string label)
    {
        if (operations.TryGetValue(label, out var operation) == false)
        {
            Debug.LogWarning($"Failed to Release({label})");
            return;
        }

        if (this.keys.Remove(label, out var keys))
        {
            foreach (var key in keys)
            {
                operations.Remove(key);
            }
        }

        Addressables.Release(operation);
    }

    public void Instantiate(string key, Action<GameObject> onComplete = null)
    {
        GameObject gameObject = Managers.Pool.Get(key);
        if (gameObject != null)
        {
            onComplete?.Invoke(gameObject);
            return;
        }

        LoadAssetAsync<GameObject>(key, original => onComplete?.Invoke(Instantiate(original)));
    }

    public GameObject Instantiate(GameObject original)
    {
        if (original == null)
            return null;
        GameObject gameObject = Object.Instantiate(original);
        gameObject.name = original.name;

        return gameObject;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<Poolable>(out var poolable))
        {
            Managers.Pool.Release(poolable);
            return;
        }

        Object.Destroy(gameObject);
    }

    public void LoadResourceLoacationAsync(string assetLabel, Action onComplete = null)
    {
        if (operations.TryGetValue(assetLabel, out var operation))
        {
            onComplete?.Invoke();
            return;
        }

        Addressables.LoadResourceLocationsAsync(assetLabel).Completed += (handle) =>
        {
            CreateGenericGroupOperation(handle, onComplete);
        };
    }
}