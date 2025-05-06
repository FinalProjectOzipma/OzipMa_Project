using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SkeletonChaseState : MyUnitStateBase
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
        //타겟이 null 일때
        if (controller.Target == null)
        {
            //Idle상태로 현재 상태 변경
            StateMachine.ChangeState(data.IdleState);
        }
        //타겟이 null이 아닐 때 
        else
        {
            if (!DetectedMap(controller.Target.transform.position))
                InnerRange(data.IdleState);
            //비활성화 되어있다면
            if (!controller.Target.activeSelf)
            {
                StateMachine.ChangeState(data.IdleState);
            }
            else
            {
                //타겟을 때릴 수 있다면
                if (IsClose())
                {
                    //전투 상태로 현재 상태 변경
                    StateMachine.ChangeState(data.AttackState);
                }
                else
                {
                    controller.Agent.SetDestination(controller.Target.transform.position);
                }
            }
        }
    }
}
