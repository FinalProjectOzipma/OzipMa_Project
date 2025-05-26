using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WarriorChaseState : WarriorStateBase
{
    public WarriorChaseState(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.autoBraking = true;
        agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        agent.SetDestination(target.transform.position);

        InnerRange(data.AttackState, status.AttackRange.GetValue());
    }
}
