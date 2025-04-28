using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireChaseState : MyUnitStateBase
{
    public VampireChaseState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!controller.Target.activeSelf || controller.Target == null)
        {
            StateMachine.ChangeState(data.IdleState);
        }
        else
        {
            if (controller.IsClose())
            {
                StateMachine.ChangeState(data.AttackState);
            }
            controller.Agent.SetDestination(controller.Target.transform.position);
        }
    }
}
