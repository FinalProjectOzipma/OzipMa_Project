using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManIdleState : ArcherManStateBase
{
    private float attackCoolDown;
    public ArcherManIdleState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
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

        if (Vector2.Distance(rigid.position, stack.Peek().transform.position) > status.AttackRange.GetValue())
            StateMachine.ChangeState(data.ChaseState);

        if (time < 0)
            StateMachine.ChangeState(data.AttackState);
    }
}
