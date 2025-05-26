using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttackState : WarriorStateBase
{
    public WarriorAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
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
    }

    public override void Update()
    {
        base.Update();
        OutRange(data.ChaseState, status.AttackRange.GetValue());

        //공격끝나면 idleState
        if (triggerCalled)
        {
            triggerCalled = false;
            StateMachine.ChangeState(data.IdleState);
        }
    }
}
