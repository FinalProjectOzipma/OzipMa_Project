using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Table = DefaultTable;
using static UnityEngine.Rendering.DebugUI;

public class ZombieIdleState : ZombieStateBase
{
    private float attackCoolDown;
    public ZombieIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, ZombieAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        attackCoolDown = status.AttackCoolDown.GetValue();
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
        time = attackCoolDown;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (target == null || !target.activeSelf)
        {
            return;
        }
        else if (!IsClose())
        {
            StateMachine.ChangeState(data.ChaseState);
        }
        else if (time < 0)
        {
            StateMachine.ChangeState(data.AttackState);
        }
    }
}
