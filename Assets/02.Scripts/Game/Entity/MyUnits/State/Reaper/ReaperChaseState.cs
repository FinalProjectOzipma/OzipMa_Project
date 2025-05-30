public class ReaperChaseState : ReaperStatebase
{
    public ReaperChaseState(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        controller.ST.FacingDir = controller.FacDir;
        agent.SetDestination(target.transform.position);

        //가까우면 공격상태로 바꿈
        if (IsClose())
        {
            StateMachine.ChangeState(data.AttackState);
        }
    }
}
