using UnityEngine;

public class KnightBody : EntityBodyBase
{
    // AttackRange
    public BoxCollider2D Slash;

    public override void Enable()
    {
        base.Enable();

        if(ctrl == null)
        {
            this.ctrl = transform.root.TryGetComponent<EnemyController>(out var ctrl) ? ctrl : null;

            // 애니메이션 데이터 생성 및 초기화
            ctrl.AnimData = new KnightAnimData();
            ctrl.AnimData.Init(ctrl);
            
            // 스탯 초기화
            ctrl.Status.Health.OnChangeHealth = healthView.SetHpBar;

            // 컨디션 초기화
            ctrl.Conditions.Add((int)AbilityType.Buff, new KnightBuff(ctrl));
        }
        Init();
    }

    public override void Init()
    {
        // 상태머신 초기화
        KnightAnimData data = ctrl.AnimData as KnightAnimData;
        ctrl.AnimData.StateMachine.ChangeState(data.ChaseState);

        base.Init();
    }
}
