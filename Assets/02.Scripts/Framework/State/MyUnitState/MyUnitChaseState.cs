using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MyUnitChaseState : MyUnitStateBase
{
    public NavMeshAgent Agent;
    public MyUnitChaseState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
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
        Anim.SetBool(animHashKey, true);
    }

    public override void Exit()
    {
        base.Exit();
        Anim.SetBool(animHashKey, false);
    }

    public override void Update()
    {
        //controller.DetectEnemyRaycast();
        //타겟이 없다면
        if (controller.Target == null)
        {
            //Idle상태로 현재 상태 변경
            StateMachine.ChangeState(data.IdleState);
        }
        //타겟이 있는데
        else
        {
            //타겟을 때릴 수 있는가
            if (controller.IsClose())
                //전투 상태로 현재 상태 변경
                StateMachine.ChangeState(data.AttackState);
        }
    }
}
