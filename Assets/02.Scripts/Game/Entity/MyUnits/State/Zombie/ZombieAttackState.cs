public class ZombieAttackState : ZombieStateBase
{
    public ZombieAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, ZombieAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = true;
        // Animator Speed 조정
        Anim.speed = Anim.GetCurrentAnimatorClipInfo(0).Length / controller.Status.AttackCoolDown.GetValue();
    }

    public override void Exit()
    {
        base.Exit();
        controller.Anim.speed = 1.0f;
    }

    public override void Update()
    {
        base.Update();
        //TODO: 
        OutRange(data.ChaseState, status.AttackRange.GetValue());
    }
}