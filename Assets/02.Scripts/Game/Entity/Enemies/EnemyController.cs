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
  

    public override void Init()
    {
        base.Init();
        //EnemyStatus = new EnemyStatus(Row);
        AnimData = new EnemyAnimationData();
        AnimData.Init(this);
    }

    public override void TakeRoot(UserObject Info)
    {
        GameObject root;
        Managers.Resource.Instantiate(Info.Name, go =>
        {
            /*MyUnit = Info as MyUnit;
            MyUnitStatus = MyUnit.Status;*/
            go.transform.SetParent(transform);
            root = go;
            Init();
        });
    }
}
