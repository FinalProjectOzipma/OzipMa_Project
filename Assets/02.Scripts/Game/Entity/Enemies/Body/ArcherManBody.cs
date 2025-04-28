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
        ctrl = GetComponentInParent<EnemyController>();
        ctrl.AnimData = new ArcherManAnimData();
        ctrl.AnimData.Init(ctrl);
        base.Init();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if(ctrl != null)
        {
            ArcherManAnimData data = ctrl.AnimData as ArcherManAnimData;
            ctrl.AnimData.StateMachine.ChangeState(data.ChaseState);
        }
    }
}
