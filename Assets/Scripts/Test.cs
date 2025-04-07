using System.Collections;
using System.Collections.Generic;
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
