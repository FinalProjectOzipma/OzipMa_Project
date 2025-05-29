public class ArcherManDeadState : ArcherManStateBase
{
    public ArcherManDeadState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
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
        if (!Anim.enabled)
        {
            Anim.enabled = true;
            OnDead(0);
            return;
        }

        if (triggerCalled)
            OnDead(0);

    }
}
