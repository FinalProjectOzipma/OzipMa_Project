using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireIdleState : VampireStateBase
{
    private float attackCoolDown;
    public VampireIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, VampireAnimationData data) : base(stateMachine, animHashKey, controller, data)
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

        else if (!DetectedMap(target.transform.position))
        {
            InnerRange(data.ChaseState);
        }
        else if (time < 0)
        {
            StateMachine.ChangeState(data.AttackState);
        }
    }
}
