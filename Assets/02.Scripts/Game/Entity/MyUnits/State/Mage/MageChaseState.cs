using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageChaseState : MageStateBase
{
    public MageChaseState(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
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
        agent.SetDestination(target.transform.position);

        //맵 감지
        if (controller.Target != null && !DetectedMap(controller.Target.transform.position))
            InnerRange(data.IdleState);

        //가까우면 공격상태로 바꿈
        if (IsClose())
        {
            StateMachine.ChangeState(data.AttackState);
        }
    }
}
