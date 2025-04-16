using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MyUnitIdleState : MyUnitStateBase
{
    private float detectRadius;
    private NavMeshAgent Agent;

    public MyUnitIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.Anim = controller.Anim;
        this.animHashKey = animHashKey;
        this.controller = controller;
        this.data = data;
        Agent = controller.Agent;
        detectRadius = controller.MyUnit.Status.AttackRange.GetValue();
        StateMachine = stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        //적감지를 함.
        GameObject target = DetectEnemyRaycast();

        //감지된 적이 있으면
        if (target != null)
        {
            // 추적 상태로 현재 상태 변경
            controller.Target = target;
            StateMachine.ChangeState(new MyUnitStateChasing(StateMachine, animHashKey, controller, data));
        }
        //감지된 적이 없는데
        else
        {
            //목적지에 도착했다면
            if (IsArrived())
                //배회 상태로 현재 상태 변경
                StateMachine.ChangeState(new MyUnitWanderingState(StateMachine, animHashKey, controller, data));
        }
    }

    //목적지에 도착했는지 확인하는 용
    public bool IsArrived()
    {
        return !Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance;
    }


    //적 감지후 감지결과를 오브젝트로 전달
    private GameObject DetectEnemyRaycast()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(controller.transform.position, detectRadius, LayerMask.GetMask("Enemy"));

        foreach (var hit in hits)
        {
            Vector2 dir = (hit.transform.position - controller.transform.position).normalized;
            float dist = Vector2.Distance(controller.transform.position, hit.transform.position);

            // 장애물 무시하고 Raycast
            RaycastHit2D ray = Physics2D.Raycast(controller.transform.position, dir, dist, LayerMask.GetMask("Enemy", "Obstacle"));

            if (ray.collider != null && ray.collider.gameObject == hit.gameObject)
            {
                return hit.gameObject;
            }
        }

        return null;
    }
}
