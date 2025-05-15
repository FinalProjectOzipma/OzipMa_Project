using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperBody : EntityBodyBase
{
    public override void Enable()
    {
        base.Enable();
        if (ctrl == null)
        {
            this.ctrl = transform.root.TryGetComponent<MyUnitController>(out var ctrl) ? ctrl : null;

            // 애니메이션 데이터 생성 및 초기화
            ctrl.AnimData = new ReaperAnimationData();
            ctrl.AnimData.Init(ctrl);

            // 스탯 초기화
            ctrl.Status.Health.OnChangeHealth = healthView.SetHpBar;

            // 컨디션 초기화
            ctrl.Conditions.TryAdd((int)AbilityType.Explosive, new ExplosiveCondition<EnemyController>(ctrl));
        }

        Init();
    }

    public override void Init()
    {
        ReaperAnimationData data = ctrl.AnimData as ReaperAnimationData;
        ctrl.AnimData.StateMachine.ChangeState(data.IdleState);
        base.Init();
    }
}
