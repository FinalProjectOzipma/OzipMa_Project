using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightChasingState : KnightStateBase
{
    public KnightChasingState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.autoBraking = true;
        agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (targets.Count <= 0) return;

        agent.SetDestination(targets.Peek().transform.position);

        InnerRange(data.AttackState, status.AttackRange.GetValue());
    }
}
