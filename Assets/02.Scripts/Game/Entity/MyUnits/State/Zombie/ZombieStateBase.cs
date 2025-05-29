public class ZombieStateBase : MyUnitStateBase
{
    protected ZombieAnimationData data;
    public ZombieStateBase(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.data = data as ZombieAnimationData;
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
