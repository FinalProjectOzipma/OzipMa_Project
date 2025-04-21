using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyStateBase
{
    public EnemyAttackState(StateMachine stateMachine, int animHashKey, EnemyController controller, EnemyAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();

        agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(rigid.position, stack.Peek().transform.position) >= status.AttackRange.GetValue())
            StateMachine.ChangeState(data.ChaseState);
    }
}
