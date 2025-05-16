using UnityEngine;
using UnityEngine.Pool;

public class Pool
{
    private GameObject original;
    private readonly Transform transform;
    private readonly ObjectPool<GameObject> poolables;

    public Pool(string key, Transform parent)
    {
        Managers.Resource.LoadAssetAsync<GameObject>(key, original => this.original = original);

        transform = new GameObject($"Pool_{key}").transform;
        transform.SetParent(parent);

        poolables = new(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    public GameObject Get()
    {
        return poolables.Get();
    }

    public void Release(Poolable poolable)
    {
        poolables.Release(poolable.gameObject);
    }

    private GameObject CreateFunc()
    {
        return Managers.Resource.Instantiate(original);
    }

    private void ActionOnGet(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    private void ActionOnRelease(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(transform);
    }

    private void ActionOnDestroy(GameObject gameObject)
    {

        Object.Destroy(gameObject);
    }
}