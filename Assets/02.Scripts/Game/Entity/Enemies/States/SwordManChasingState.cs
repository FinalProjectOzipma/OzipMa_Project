using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordManChasingState : EnemyStateBase
{
    public SwordManChasingState(StateMachine stateMachine, int animHashKey, EnemyController controller, SwordManAnimData data) : base(stateMachine, animHashKey, controller, data)
    {
        agent.autoBraking = true;
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

        if (stack.Count <= 0) return;

        agent.SetDestination(stack.Peek().transform.position);

        //if (Vector2.Distance(rigid.position, stack.Peek().transform.position) <= status.AttackRange.GetValue())
            //StateMachine.ChangeState(data.AttackState);
    }


}
