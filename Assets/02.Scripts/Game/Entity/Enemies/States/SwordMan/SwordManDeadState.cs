public class SwordManDeadState : SwordManStateBase
{
    public SwordManDeadState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
        triggerCalled = false;
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
        {
            if (controller.gameObject.activeInHierarchy)
                OnDead(0);

        }
    }
}
