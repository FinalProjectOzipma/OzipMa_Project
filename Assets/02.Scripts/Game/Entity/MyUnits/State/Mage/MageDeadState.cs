using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageDeadState : MageStateBase
{
    public MageDeadState(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
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
