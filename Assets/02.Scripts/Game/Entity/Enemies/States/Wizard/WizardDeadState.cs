public class WizardDeadState : WizardStateBase
{
    public WizardDeadState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
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
        if (triggerCalled)
        {
            if (controller.gameObject.activeInHierarchy)
                OnDead(1);

        }
    }
}
