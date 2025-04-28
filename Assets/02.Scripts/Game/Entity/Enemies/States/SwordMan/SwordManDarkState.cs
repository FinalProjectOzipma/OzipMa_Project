using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManDarkState : SwordManStateBase
{
    private float darkCoolDown = 5f;
    public SwordManDarkState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        DeBuff(darkCoolDown, 0.5f, AbilityType.Dark);
    }

    public override void Exit()
    {
        base.Exit();
        ExitDeBuff(AbilityType.None);
    }

    public override void Update()
    {
        base.Update();

        if (time < 0)
            StateMachine.ChangeState(data.ChaseState);
        else
        {
            //agent.SetDestination((Vector2)(-targets.Peek().transform.position));
            Vector2 dir = (targets.Peek().transform.position) - transform.position;
            agent.Move(-dir.normalized * status.MoveSpeed.GetValue() * 0.5f * Time.deltaTime);
            controller.FlipControll();
        }
    }
}
