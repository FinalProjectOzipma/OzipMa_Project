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

        FakeStart();
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

    //적 감지후 감지결과를 오브젝트로 전달
    public void DetectEnemyRaycast()
    {
        //float detectRadius = MyUnitStatus.AttackRange;
        float detectRadius = 1.0f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRadius, LayerMask.GetMask("Enemy"));

        foreach (var hit in hits)
        {
            Vector2 dir = (hit.transform.position - transform.position).normalized;
            float dist = Vector2.Distance(transform.position, hit.transform.position);

            // 장애물 무시하고 Raycast
            RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, dist, LayerMask.GetMask("Enemy", "Obstacle"));

            if (ray.collider != null && ray.collider.gameObject == hit.gameObject)
            {
                Target = hit.gameObject;
            }
        }
    }
}