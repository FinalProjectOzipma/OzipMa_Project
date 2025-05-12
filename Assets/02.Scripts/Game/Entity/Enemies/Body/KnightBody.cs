public class KnightBody : EntityBodyBase
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        if(ctrl == null)
        {
            ctrl = GetComponentInParent<EnemyController>();
            ctrl.AnimData = new KnightAnimData();
            ctrl.AnimData.Init(ctrl);
            ctrl.Status.Health.OnChangeHealth = healthView.SetHpBar;
        }
        base.Init();
    }

    public override void Enable()
    {
        base.Enable();

        if(ctrl != null)
        {
            KnightAnimData data = ctrl.AnimData as KnightAnimData;
            ctrl.AnimData.StateMachine.ChangeState(data.ChaseState);
            Init();
        }
    }
}
