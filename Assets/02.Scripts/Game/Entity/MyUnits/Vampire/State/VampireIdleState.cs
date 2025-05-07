using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireIdleState : VampireStateBase
{
    public VampireIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, VampireAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (controller.Target == null)
        {
            SetTarget();
        }
        else
        {
            if (!controller.Target.activeSelf)
            {
                SetTarget();
            }
        }
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
            SetTarget();
        }
        else
        {
            if (controller.Target.activeSelf)
            {
                if (IsClose())
                {
                    StateMachine.ChangeState(data.AttackState);
                }
                else
                {
                    StateMachine.ChangeState(data.ChaseState);
                }
            }
            else
            {
                SetTarget();
            }
        }
    }
}
