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

        if(targets.Peek() == core && agent.remainingDistance > 0.1f)
        {
            StateMachine.ChangeState(data.ChaseState);
            return;
        }    

        if (time < 0)
            StateMachine.ChangeState(data.AttackState);
    }
}
