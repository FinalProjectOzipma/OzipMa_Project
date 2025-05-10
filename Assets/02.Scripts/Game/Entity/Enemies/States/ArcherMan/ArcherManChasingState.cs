using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManChasingState : ArcherManStateBase
{
    public ArcherManChasingState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = false;

        controller.SpTrail.SetActive(true, null);
    }

    public override void Exit()
    {
        base.Exit();
        controller.SpTrail.SetActive(false, null);
    }

    public override void Update()
    {
        base.Update();

        controller.SpTrail.FacingDir = controller.FacDir;

        agent.SetDestination(targets.Peek().transform.position);
        
        if(!DetectedMap(targets.Peek().transform.position))
            InnerRange(data.IdleState);

        /*if(targets.Peek() == core)
        {
            agent.isStopped = false;
            if (agent.remainingDistance < 0.1f)
                StateMachine.ChangeState(data.AttackState);

            return;
        }
        else
        {
            agent.isStopped = true;
            OutRange(data.AttackState);
        }*/

    }
}
