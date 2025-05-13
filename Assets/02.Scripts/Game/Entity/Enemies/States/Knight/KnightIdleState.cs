using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightIdleState : KnightStateBase
{
    private float skillCoolDown = 2f;
    public KnightIdleState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
        time = skillCoolDown;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        int rand = Random.Range(0, 2);
        
        if (time <= 0f)
        {
            if(rand == 0)
            {
                if (controller.ConditionHandlers[(int)AbilityType.Buff].CurDuration <= 0f)
                {
                    StateMachine.ChangeState(data.AtkBuffState);
                    return;
                }
            }

            StateMachine.ChangeState(data.ChaseState);
        }
    }
}
