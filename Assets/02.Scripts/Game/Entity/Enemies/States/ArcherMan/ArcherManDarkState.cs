using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManDarkState : ArcherManStateBase
{
    private float darkCoolDown = 5f;
    private ConditionHandler handler;
    public ArcherManDarkState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {   
    }

    public override void Enter()
    {
        base.Enter();
        Anim.speed = 0.5f;
        time = darkCoolDown;
        handler = controller.ConditionHandlers[(int)AbilityType.Dark];
        handler.ObjectActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        Anim.speed = 1f;
        handler.ObjectActive(false);
    }

    public override void Update()
    {
        base.Update();

        if(time < 0)
            StateMachine.ChangeState(data.ChaseState);
        else
        {
            //agent.SetDestination((Vector2)(-targets.Peek().transform.position));
            Vector2 dir = (handler.Attacker.position) - transform.position;
            agent.Move(-dir.normalized * status.MoveSpeed.GetValue() * 0.2f * Time.deltaTime);
            controller.FlipControll();
        }
    }
}
