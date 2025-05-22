public class ArcherManChasingState : ArcherManStateBase
{
    ConditionHandler handler;
    public ArcherManChasingState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        ConditionHandler handler = controller.ConditionHandlers[(int)AbilityType.Dark];

        if (targets.Count <= 0) return;

        if(!handler.IsPlaying) agent.SetDestination(targets.Peek().transform.position);

        if (targets.Peek().layer == (int)Enums.Layer.Core)
        {
            if (agent.remainingDistance <= 0f)
                StateMachine.ChangeState(data.AttackState);
            return;
        }

        if (!DetectedMap(targets.Peek().transform.position))
            InnerRange(data.IdleState);
    }
}
