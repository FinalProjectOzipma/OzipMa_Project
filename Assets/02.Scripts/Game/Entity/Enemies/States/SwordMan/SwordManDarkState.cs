using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManDarkState : SwordManStateBase
{
    private float darkCoolDown = 5f;
    private ConditionHandler handler;
    public SwordManDarkState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();

        time = darkCoolDown;
        anim.speed = 0.5f;
        handler = controller.ConditionHandlers[(int)AbilityType.Dark];
        handler.ObjectActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        anim.speed = 1f;
        handler.ObjectActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (time < 0)
            StateMachine.ChangeState(data.ChaseState);
        else
        {
            agent.SetDestination(handler.Attacker.position);
            controller.FlipControll();
        }
    }
}
