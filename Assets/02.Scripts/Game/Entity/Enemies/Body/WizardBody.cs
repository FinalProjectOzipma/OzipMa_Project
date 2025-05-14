using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBody : EntityBodyBase
{
    public override void Enable()
    {
        base.Enable();
        if (ctrl == null)
        {
            this.ctrl = transform.root.TryGetComponent<EnemyController>(out var ctrl) ? ctrl : null;

            // 애니메이션 데이터 생성 및 초기화
            ctrl.AnimData = new WizardAnimData();
            ctrl.AnimData.Init(ctrl);            
            
            // 스탯 초기화
            ctrl.Status.Health.OnChangeHealth = healthView.SetHpBar;
        }

        Init();
    }

    public override void Init()
    {
        // 상태머신 초기화
        WizardAnimData data = ctrl.AnimData as WizardAnimData;
        ctrl.AnimData.StateMachine.ChangeState(data.IdleState);

        base.Init();
    }
}
