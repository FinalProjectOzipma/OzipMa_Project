public class KnightAtkBuffState : KnightStateBase
{
    private bool isBuffed;
    public KnightAtkBuffState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
        isBuffed = false;
        Managers.Audio.PlaySFX(SFXClipName.PowerUp);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled && isBuffed)
        {
            StateMachine.ChangeState(data.IdleState);
            return;
        }

        if (triggerCalled && !isBuffed)
        {
            isBuffed = true;
            triggerCalled = false;

            controller.ConditionHandlers[(int)AbilityType.Buff].ObjectActive(true);
            controller.ApplyCondition(AbilityType.Buff, 0f, controller.gameObject);
        }
    }
}
