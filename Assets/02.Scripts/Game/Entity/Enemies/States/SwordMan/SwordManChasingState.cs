public class SwordManChasingState : SwordManStateBase
{
    public SwordManChasingState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.autoBraking = true;
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

        InnerRange(data.AttackState, status.AttackRange.GetValue());

    }
}
