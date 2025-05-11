using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBody : EntityBodyBase
{
    public override void Enable()
    {
        base.Enable();
        if (ctrl != null)
        {
            ZombieAnimationData data = ctrl.AnimData as ZombieAnimationData;
            ctrl.AnimData.StateMachine.ChangeState(data.IdleState);
        }
    }
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        if (ctrl == null)
        {
            ctrl = GetComponentInParent<MyUnitController>();
            ctrl.AnimData = new ZombieAnimationData();
            ctrl.AnimData.Init(ctrl);
            ctrl.Status.Health.OnChangeHealth = healthView.SetHpBar;
        }
        base.Init();
    }
}
