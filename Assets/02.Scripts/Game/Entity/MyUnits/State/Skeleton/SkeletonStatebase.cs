public class SkeletonStateBase : MyUnitStateBase
{
    protected SkeletonAnimationData data;
    public SkeletonStateBase(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.data = data as SkeletonAnimationData;
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
        if (Managers.Wave.CurEnemyList.Count == 0)
            StateMachine.ChangeState(data.IdleState);
    }
}