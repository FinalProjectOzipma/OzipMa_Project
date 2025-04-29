using DefaultTable1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SwordManBody : EnemyBodyBase
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
            ctrl.AnimData = new SwordManAnimData();
            ctrl.AnimData.Init(ctrl);
            ctrl.Status.Health.OnChangeHealth =  healthView.SetHpBar;
        }
        
        base.Init();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (ctrl != null)
        {
            SwordManAnimData data = ctrl.AnimData as SwordManAnimData;
            ctrl.AnimData.StateMachine.ChangeState(data.ChaseState);
            Init();
        }
    }
}
