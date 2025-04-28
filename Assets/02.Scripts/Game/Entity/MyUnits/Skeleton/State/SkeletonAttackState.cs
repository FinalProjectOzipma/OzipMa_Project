using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : MyUnitStateBase
{
    public SkeletonAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (controller.Target == null)
        {
            StateMachine.ChangeState(data.IdleState);
        }
        else
        {
            if (controller.Target.activeSelf)
            {
                //타겟이 범위 내에 없다면
                if (!controller.IsClose())
                    //추격 상태로 현재 상태 변경
                    StateMachine.ChangeState(data.ChaseState);
            }
            else
            {
                //탐색 상태로 현재 상태 변경
                StateMachine.ChangeState(data.IdleState);
            }
        }
    }
}