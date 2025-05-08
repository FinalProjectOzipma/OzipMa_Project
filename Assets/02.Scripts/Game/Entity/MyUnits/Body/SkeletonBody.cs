using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBody : EntityBodyBase
{
    public override void Enable()
    {
        base.Enable();
        if (ctrl != null)
        {
            SkeletonAnimationData data = ctrl.AnimData as SkeletonAnimationData;
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
            ctrl.AnimData = new SkeletonAnimationData();
            ctrl.AnimData.Init(ctrl);
            ctrl.Status.Health.OnChangeHealth = healthView.SetHpBar;
        }
        base.Init();
    }
}
