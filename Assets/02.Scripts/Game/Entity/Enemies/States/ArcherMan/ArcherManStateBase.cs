using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManStateBase : EnemyStateBase
{
    protected ArcherManAnimData data;
    public ArcherManStateBase(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.data = data as ArcherManAnimData;
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

        if (controller.IsDead)
            return;

        if (status.Health.GetValue() <= 0.0f)
        {
            controller.StopAllCoroutines();
            controller.IsDead = true;
            StateMachine.ChangeState(data.DeadState);
        } 
    }

    public void InnerRange(ArcherManStateBase nextState)
    {
        if (Vector2.Distance(transform.position, targets.Peek().transform.position) <= status.AttackRange.GetValue())
            StateMachine.ChangeState(nextState);
    }
}
