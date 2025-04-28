using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireChaseState : MyUnitStateBase
{
    public VampireChaseState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
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
        if (controller.Target == null)
        {
            StateMachine.ChangeState(data.IdleState);
        }
        else
        {
            //비활성화 되어있다면
            if (!controller.Target.activeSelf)
            {
                StateMachine.ChangeState(data.IdleState);
            }
            //활성화되어있다면
            else
            {
                if (controller.IsClose())
                {
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
