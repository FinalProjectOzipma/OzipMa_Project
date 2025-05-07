using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeadState : ZombieStateBase
{
    public ZombieDeadState(StateMachine stateMachine, int animHashKey, MyUnitController controller, ZombieAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = true;
        Managers.Wave.CurMyUnitList.Remove(controller.gameObject);
        controller.Target = null;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (triggerCalled)
            Managers.Resource.Destroy(controller.gameObject);
    }
}
