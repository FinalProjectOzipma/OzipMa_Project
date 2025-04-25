using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : MyUnitStateBase
{
    public SkeletonDeadState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }
    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = true;
        triggerCalled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            Managers.Resource.Destroy(controller.gameObject);
    }
}
