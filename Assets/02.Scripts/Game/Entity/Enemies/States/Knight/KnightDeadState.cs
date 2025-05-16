using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightDeadState : KnightStateBase
{
    public KnightDeadState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
        triggerCalled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (triggerCalled)
        {
            if (controller.gameObject.activeInHierarchy)
                OnDead(1);
            
        }
    }
}
