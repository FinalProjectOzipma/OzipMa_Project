public class VampireStateBase : MyUnitStateBase
{
    protected VampireAnimationData data;
    public VampireStateBase(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.data = data as VampireAnimationData;
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

    /// <summary>
    /// 흡혈
    /// </summary>
    public void Heal()
    {
        if (target == null || !target.activeSelf)
        {
            return;
        }
        float atkValue = controller.MyUnit.Status.Attack.GetValue();
        float defence = target.GetComponent<EnemyController>().Enemy.Status.Attack.GetValue();
        float damageScale = atkValue / (atkValue + defence);

        float amount = atkValue * damageScale;
        controller.Status.Health.AddValue(amount);
    }
}
