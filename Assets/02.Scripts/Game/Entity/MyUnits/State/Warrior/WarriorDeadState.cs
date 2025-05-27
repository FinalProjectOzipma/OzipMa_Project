using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorDeadState : WarriorStateBase
{
    public WarriorDeadState(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = true;
        controller.Target = null;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (triggerCalled)
            OnDead();
    }
}
