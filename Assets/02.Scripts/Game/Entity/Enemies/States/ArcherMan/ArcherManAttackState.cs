using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArcherManAttackState : ArcherManStateBase
{
    private Action<GameObject> CreateArrow;
    private string Arrow = nameof(Arrow);

    public ArcherManAttackState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data, Action<GameObject> CreateArrow) : base(stateMachine, animHashKey, controller, data)
    {
        this.CreateArrow = CreateArrow;
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
        projectileCalled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (DetectedMap(targets.Peek().transform.position))
            StateMachine.ChangeState(data.ChaseState);

        OutRange(data.ChaseState);

        if(projectileCalled) // 화살 만드는 Attack구간
        {
            CreateArrow?.Invoke(targets.Peek());
            projectileCalled = false;
        }

        if (triggerCalled) // 공격 모션이 끝나는 구간
            StateMachine.ChangeState(data.IdleState);
    }
}
