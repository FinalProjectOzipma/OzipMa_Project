using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManChasingState : ArcherManStateBase
{
    public ArcherManChasingState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        agent.SetDestination(stack.Peek().transform.position);

        if (Vector2.Distance(transform.position, stack.Peek().transform.position) <= status.AttackRange.GetValue())
            StateMachine.ChangeState(data.IdleState);
    }
}
