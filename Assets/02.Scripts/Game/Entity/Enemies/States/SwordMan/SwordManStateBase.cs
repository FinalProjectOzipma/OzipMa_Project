public class SwordManStateBase : EnemyStateBase
{
    protected SwordManAnimData data;
    public SwordManStateBase(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.data = data as SwordManAnimData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (DeadCheck())
        {
            StateMachine.ChangeState(data.DeadState);
            return;
        }
    }
}
