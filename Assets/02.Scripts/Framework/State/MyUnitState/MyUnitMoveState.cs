using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MyUnitMoveState : MyUnitStateBase
{
    public NavMeshAgent Agent;
    public MyUnitMoveState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        StateMachine = stateMachine;
        this.Anim = controller.Anim;
        this.controller = controller;
        this.data = data;
        this.animHashKey = animHashKey;
        Agent = controller.Agent;
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
        controller.DetectEnemyRaycast();
        //감지된 적이 있으면
        if (controller.Target != null)
        {
            StateMachine.ChangeState(data.ChaseState);
        }
        //감지된 적이 없는데
        else
        {
            //목적지에 도착했다면
            if (IsArrived())
                //정지 상태로 현재 상태 변경
                StateMachine.ChangeState(data.IdleState);
        }
    }

    //목적지에 도착했는지 확인하는 용
    public bool IsArrived()
    {
        return !Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance;
    }
}
