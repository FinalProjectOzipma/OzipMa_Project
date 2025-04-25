using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WizardChasingState : WizardStateBase
{
    public WizardChasingState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = false;
        if (wave.CurMyUnitList.Count > 0)
        {
            int rand = Random.Range(0, wave.CurMyUnitList.Count);
            targets.Push(wave.CurMyUnitList[rand]);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        agent.SetDestination(targets.Peek().transform.position);

        if (!DetectedMap(targets.Peek().transform.position))
            InnerRange(data.AttackState);
    }
}
