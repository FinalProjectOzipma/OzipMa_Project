using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManDarkState : ArcherManStateBase
{
    private float darkCoolDown = 2f;
    public ArcherManDarkState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();

        time = darkCoolDown;
        anim.speed = 0.5f;
        controller.Conditions[(int)AbilityType.Dark].ObjectActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        anim.speed = 1f;
        controller.CurrentCondition = AbilityType.None;
        controller.Conditions[(int)AbilityType.Dark].ObjectActive(false);
    }

    public override void Update()
    {
        base.Update();

        if(time < 0)
            StateMachine.ChangeState(data.ChaseState);
        else
            agent.SetDestination(-targets.Peek().transform.position);
    }
}
