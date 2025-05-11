using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class VampireBody : EntityBodyBase
{
    public override void Enable()
    {
        base.Enable();
        if (ctrl != null)
        {
            VampireAnimationData data = ctrl.AnimData as VampireAnimationData;
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