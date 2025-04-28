using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManDeadState : ArcherManStateBase
{
    public ArcherManDeadState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
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
        base.Update();

        if (triggerCalled)
        {
            if (controller.gameObject.activeInHierarchy)
                Managers.Resource.Destroy(controller.gameObject);
        }
    }
}
