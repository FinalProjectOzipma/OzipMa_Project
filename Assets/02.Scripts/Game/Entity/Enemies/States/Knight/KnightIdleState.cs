using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightIdleState : KnightStateBase
{
    private float skillCoolDown = 2f;
    public KnightIdleState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
        time = skillCoolDown;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(time <= 0f)
            StateMachine.ChangeState(data.ChaseState);
    }
}
