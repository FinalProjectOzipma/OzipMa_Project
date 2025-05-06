using Cysharp.Threading.Tasks;
using DefaultTable1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class ArcherManBody : EnemyBodyBase
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
            ctrl.AnimData = new ArcherManAnimData();
            ctrl.AnimData.Init(ctrl);
            ctrl.Status.Health.OnChangeHealth = healthView.SetHpBar;
        }
        base.Init();
    }

    public override void Enable()
    {
        base.Enable();

        if(ctrl != null)
        {
            ArcherManAnimData data = ctrl.AnimData as ArcherManAnimData;
            ctrl.AnimData.StateMachine.ChangeState(data.ChaseState);
            Init();
        }
    }
}
