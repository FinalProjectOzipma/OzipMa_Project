public class ZombieChaseState : ZombieStateBase
{
    public ZombieChaseState(StateMachine stateMachine, int animHashKey, MyUnitController controller, ZombieAnimationData data) : base(stateMachine, animHashKey, controller, data)
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

        agent.SetDestination(target.transform.position);

        InnerRange(data.AttackState, status.AttackRange.GetValue());
    }
}
