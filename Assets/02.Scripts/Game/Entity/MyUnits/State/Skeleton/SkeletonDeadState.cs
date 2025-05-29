public class SkeletonDeadState : SkeletonStateBase
{
    public SkeletonDeadState(StateMachine stateMachine, int animHashKey, MyUnitController controller, SkeletonAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //TODO: 아군 죽는 사운드 필요
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (triggerCalled)
            OnDead();
    }
}
