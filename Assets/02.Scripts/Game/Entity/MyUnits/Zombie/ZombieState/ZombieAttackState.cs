using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackState : MyUnitStateBase
{
    public ZombieAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
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
        base.Update();
        //타겟이 비어있다면
        if (controller.Target == null)
        {
            StateMachine.ChangeState(data.IdleState);
        }
        else
        {
            //타겟이 비활성화되어있다면
            if (controller.Target.activeSelf)
            {
                StateMachine.ChangeState(data.IdleState);
            }
            else
            {
                //타겟을 때릴 수 있는가
                if (!controller.IsClose())
                    //전투 상태로 현재 상태 변경
                    StateMachine.ChangeState(data.ChaseState);
            }
        }
    }
}
