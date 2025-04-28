using DefaultTable1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SwordManBody : EnemyBodyBase
{
    public override void Init()
    {
        base.Init();
        ctrl.AnimData = new SwordManAnimData();
        ctrl.AnimData.Init(ctrl);
    }
}
