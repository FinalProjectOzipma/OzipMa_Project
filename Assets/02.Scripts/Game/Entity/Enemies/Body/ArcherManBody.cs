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
    public override void Init()
    {
        base.Init();
        ctrl.AnimData = new ArcherManAnimData();
        ctrl.AnimData.Init(ctrl);
        
    }
}
