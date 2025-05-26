using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorIdleState : WarriorStateBase
{
    private float attackCoolDown;

    public WarriorIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        attackCoolDown = status.AttackCoolDown.GetValue();
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;

        time = attackCoolDown;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Managers.Wave.CurEnemyList.Count == 0)
            return;
        else if (!IsClose())
        {
            StateMachine.ChangeState(data.ChaseState);
        }
        else if (time < 0)
        {
            StateMachine.ChangeState(data.AttackState);
        }
    }
}
