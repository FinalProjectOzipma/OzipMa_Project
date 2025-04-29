using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Table = DefaultTable;
using static UnityEngine.Rendering.DebugUI;

public class ZombieIdleState : MyUnitStateBase
{
    public ZombieIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, ZombieAnimationData data) : base(stateMachine, animHashKey, controller, data)
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
