using UnityEngine;

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
            targets.Push(wave.CurMyUnitList[rand].gameObject);
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

        if (targets.Peek().layer == (int)Enums.Layer.Core)
        {
            if (agent.remainingDistance <= 0f)
                StateMachine.ChangeState(data.AttackState);
            return;
        }

        if (!DetectedMap(targets.Peek().transform.position))
            InnerRange(data.AttackState);
    }
}
