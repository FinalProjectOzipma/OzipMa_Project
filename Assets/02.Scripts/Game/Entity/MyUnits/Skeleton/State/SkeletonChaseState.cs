using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SkeletonChaseState : SkeletonStateBase
{
    public SkeletonChaseState(StateMachine stateMachine, int animHashKey, MyUnitController controller, SkeletonAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //타겟이 없을 때
        if (target == null || !target.activeSelf)
        {
            //Idle상태로 현재 상태 변경
            StateMachine.ChangeState(data.IdleState);
        }
        //타겟이 있을때 
        else
        {
            //맵 감지
            if (!DetectedMap(controller.Target.transform.position))
                InnerRange(data.IdleState);
            //가까우면 공격상태로 바꿈
            if (IsClose())
            {
                StateMachine.ChangeState(data.AttackState);
            }
        }

        controller.Agent.SetDestination(controller.Target.transform.position);
    }
}
