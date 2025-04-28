using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManAttackState : SwordManStateBase
{
    public SwordManAttackState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
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

        OutRange(data.ChaseState, status.AttackRange.GetValue());
    }
}
