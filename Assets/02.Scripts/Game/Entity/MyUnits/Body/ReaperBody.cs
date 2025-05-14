using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperBody : EntityBodyBase
{
    public override void Enable()
    {
        base.Enable();
        if (ctrl != null)
        {
            ReaperAnimationData data = ctrl.AnimData as ReaperAnimationData;
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
            ctrl.AnimData = new VampireAnimationData();
            ctrl.AnimData.Init(ctrl);
            ctrl.Status.Health.OnChangeHealth = healthView.SetHpBar;
        }
        base.Init();
    }
}
