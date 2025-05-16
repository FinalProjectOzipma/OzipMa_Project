public class ArcherManBody : EntityBodyBase
{
    public override void Enable()
    {
        base.Enable();

        if(ctrl == null)
        {
            this.ctrl = transform.root.TryGetComponent<EnemyController>(out var ctrl) ? ctrl : null;

            // 애니메이션 데이터 생성 및 초기화
            ctrl.AnimData = new ArcherManAnimData();
            ctrl.AnimData.Init(ctrl);

            // 컨디션 초기화
            ctrl.Conditions.TryAdd((int)AbilityType.Explosive, new ExplosiveCondition<EnemyController>(ctrl));
        }
        // 스탯 초기화
        ctrl.Status.Health.OnChangeHealth -= healthView.SetHpBar;
        ctrl.Status.Health.OnChangeHealth += healthView.SetHpBar;

        healthView.SetHpBar(ctrl.Status.Health.GetValue(), ctrl.Status.Health.MaxValue);
        Init();
    }

    public override void Init()
    {
        // 상태머신 초기화
        ArcherManAnimData data = ctrl.AnimData as ArcherManAnimData;
        ctrl.AnimData.StateMachine.ChangeState(data.ChaseState);

        base.Init();
    }
}
