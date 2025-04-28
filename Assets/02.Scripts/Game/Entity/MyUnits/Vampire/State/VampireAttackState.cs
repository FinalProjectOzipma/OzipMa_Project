using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireAttackState : MyUnitStateBase
{
    public VampireAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = true;
    }


    public override void Exit()
    {
        base.Exit();
        if (!controller.Target.activeSelf || controller.Target == null)
        {
            StateMachine.ChangeState(data.IdleState);
        }
        else
        {
            if (!controller.IsClose())
            {
                StateMachine.ChangeState(data.ChaseState);
            }
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
