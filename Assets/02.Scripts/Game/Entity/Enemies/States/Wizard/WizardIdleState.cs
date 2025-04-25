using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardIdleState : WizardStateBase
{
    private float skillCoolDown = 2f;
    public WizardIdleState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
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

        if(time < 0f)
        {
            int rand = 1; //Random.Range(0, 2);

            if (rand == 1)
                StateMachine.ChangeState(data.ChaseState);
            else
                StateMachine.ChangeState(data.LightningState);
        }
    }
}
