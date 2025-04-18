using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MyUnitIdleState : MyUnitStateBase
{
    private NavMeshAgent Agent;

    public MyUnitIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.Anim = controller.Anim;
        //this.Rigid = controller.Rigid;
        this.animHashKey = animHashKey;
        this.controller = controller;
        this.data = data;
        Agent = controller.Agent;
        StateMachine = stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        Anim.SetBool(animHashKey, true);
        if (controller.Target == null)
        {
            SetTarget();
        }
    }

    public override void Exit()
    {
        base.Exit();
        Anim.SetBool(animHashKey, false);
    }

    public override void Update()
    {
        base.Update();
        if (controller.Target != null)
        {
            if (controller.IsClose())
            {
                StateMachine.ChangeState(data.AttackState);
            }
            else
            {
                StateMachine.ChangeState(data.ChaseState);
            }
        }
    }

    // TODO: 타겟 지정 메서드 만들기
    public void SetTarget()
    {

        //Managers.Wave에서 남은 적 리스트 가져오기
        //적이 없으면 그냥 리턴해버리기
        //적들과의 거리를 비교하고 그중 가장 거리가 가까운 적을 타겟으로 지정
    }
}
