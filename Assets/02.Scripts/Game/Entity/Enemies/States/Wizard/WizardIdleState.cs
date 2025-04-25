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
        time = skillCoolDown;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(time < 0)
        {
            int rand = Random.Range(0, 2);

            if (rand == 1)
                StateMachine.ChangeState(data.AttackState);
            else
                StateMachine.ChangeState(data.LightningState);
        }
    }
}
