public class KnightDeadState : KnightStateBase
{
    public KnightDeadState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
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
            Anim.enabled = true; // 이것도 안되면 그냥 바로 Destroy
            OnDead(0);
            return;
        }

        if (triggerCalled)
        {
            if (controller.gameObject.activeInHierarchy)
                OnDead(1);

        }
    }
}
