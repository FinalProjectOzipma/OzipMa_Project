public class SwordManAnimData : EntityAnimationData
{
    public SwordManAttackState AttackState { get; private set; }
    public SwordManChasingState ChaseState { get; private set; }
    public SwordManDeadState DeadState { get; private set; }
    public SwordManDarkState DarkState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init(controller);
        AttackState = new SwordManAttackState(StateMachine, AttackHash, controller as EnemyController, this);
        ChaseState = new SwordManChasingState(StateMachine, ChaseHash, controller as EnemyController, this);
        DeadState = new SwordManDeadState(StateMachine, DeadHash, controller as EnemyController, this);
        DarkState = new SwordManDarkState(StateMachine, DarkHash, controller as EnemyController, this);
        controller.Conditions.Add((int)AbilityType.Dark, new DarkCondition<SwordManStateBase>(StateMachine, ChaseState, DarkState, controller as EnemyController));

        StateMachine.Init(ChaseState);
    }
}
