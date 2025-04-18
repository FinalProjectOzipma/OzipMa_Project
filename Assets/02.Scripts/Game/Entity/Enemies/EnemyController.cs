using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController
{
    public EnemyStatus EnemyStatus { get; private set; }

    public NavMeshAgent Agent;

    public GameObject Target;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }
  

    public override void Init(int primaryKey, string name, Vector2 position, GameObject gameObject = null)
    {
        base.Init(primaryKey, name, position);
        //EnemyStatus = new EnemyStatus(Row);
        AnimData = new EnemyAnimationData();
        AnimData.Init(this);
    }

    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        Managers.Resource.Instantiate(Name, go =>
        {
            go.transform.SetParent(transform);
            Init(primaryKey, name, position, go);
        });
    }
}
