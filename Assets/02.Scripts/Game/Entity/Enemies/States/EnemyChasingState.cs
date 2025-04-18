using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyStateBase
{
    public EnemyChasingState(StateMachine stateMachine, int animHashKey, EnemyController controller, EnemyAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.stoppingDistance = 0.5f;
        agent.autoBraking = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        // 베이스에 업데이트 아무것도 없기때문에 베이스는 실행 안해준다
        //base.Update();

        if (controller.Target == null) return;

        agent.SetDestination(controller.Target.transform.position);

        if (rigid.velocity.sqrMagnitude <= 0.01f)
            StateMachine.ChangeState(data.AttackState);
    }


}
