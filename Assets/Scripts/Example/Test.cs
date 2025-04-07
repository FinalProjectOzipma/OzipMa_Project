using DefaultTable;
using GoogleSheet.Core.Type;
using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Test : MonoBehaviour
{
    public GameObject prefab;
    public Queue<GameObject> queue;

    private void Awake()
    {
        queue = new Queue<GameObject>();
    }

    private void Start()
    {
        int a = DefaultTable.Data.DataList.Count;
        UnityGoogleSheet.LoadFromGoogle<int, DefaultTable.Data>((list, map) =>
        {
            foreach (var ii in list)
            {
                Debug.Log($"{ii.enumTest.Values[0]} {ii.enumTest.Values[1]}");
            }
        }, true);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Managers.Resource.Instantiate("Circle", go => queue.Enqueue(go));
            Managers.Resource.Instantiate("Square", go => queue.Enqueue(go));

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Managers.Resource.Destroy(queue.Dequeue());
        }
    }

}