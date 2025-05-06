using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBody : EntityBodyBase
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
            ctrl.AnimData = new WizardAnimData();
            ctrl.AnimData.Init(ctrl);
            ctrl.Status.Health.OnChangeHealth = healthView.SetHpBar;
        }
        base.Init();
    }

    public override void Enable()
    {
        base.Enable();
        if (ctrl != null)
        {
            WizardAnimData data = ctrl.AnimData as WizardAnimData;
            ctrl.AnimData.StateMachine.ChangeState(data.IdleState);
        }
    }
}
