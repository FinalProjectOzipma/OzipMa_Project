using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyUnitController : EntityController
{
    public MyUnit MyUnit { get; private set; }
    public MyUnitStatus MyUnitStatus { get; private set; }
    public NavMeshAgent Agent;

    public GameObject Target;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        //// 테스트용 기본값 설정
        //Agent.speed = 3.5f;
        //Agent.acceleration = 12f;
        //Agent.stoppingDistance = 0.1f;

        FakeStart();
    }

    // 테스트용: 시작 후 우측 위 방향으로 이동 시도
    private void Start()
    {
        Vector2 testTarget = (Vector2)transform.position + new Vector2(-3, -3);

        if (NavMesh.SamplePosition(new Vector3(testTarget.x, testTarget.y, 0), out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            Debug.Log($"Sampled Position: {hit.position}");
            Agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning("NavMesh 샘플 위치 없음");
        }
    }

    private void FakeStart()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        AnimData = new MyUnitAnimationData();
        AnimData.Init(this);
    }

    public override void TakeRoot(UserObject Info)
    {
        GameObject root;
        Managers.Resource.Instantiate(nameof(Info), go =>
        {
            MyUnit = Info as MyUnit;
            MyUnitStatus = MyUnit.Status;
            go.transform.SetParent(transform);
            root = go;
            Init();
        });
    }
}
